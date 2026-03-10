#!/bin/bash

set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
TEMPLATE_FILE="${SCRIPT_DIR}/cloudformation/stack.yaml"
STACK_NAME="image-serving-stack"

usage() {
    echo "Usage: $0 -b <bucket-name> -d <domain-name> [-r <aws-region>]"
    echo ""
    echo "Deploy CloudFormation stack for image serving with S3 + CloudFront + Route53 + ACM"
    echo ""
    echo "NOTE: This stack must be deployed in us-east-1 (ACM for CloudFront requires it)."
    echo "      After deployment, add the output NameServers as NS records in CloudFlare."
    echo ""
    echo "Options:"
    echo "  -b, --bucket-name    S3 bucket name (required, must be globally unique)"
    echo "  -d, --domain-name    Custom domain name (required, e.g., images.example.com)"
    echo "  -r, --region         AWS region (default: from AWS CLI config)"
    echo "  -h, --help           Show this help message"
    echo ""
    echo "Example:"
    echo "  $0 -b my-images-bucket -d images.example.com -r us-east-1"
    exit 1
}

validate_bucket_name() {
    local bucket="$1"
    if [[ ! "$bucket" =~ ^[a-z0-9][a-z0-9-]{1,61}[a-z0-9]$ ]]; then
        echo "Error: Bucket name must be 3-63 characters, lowercase, numbers, and hyphens."
        echo "       Must start and end with a letter or number."
        exit 1
    fi
}

validate_domain_name() {
    local domain="$1"
    if [[ ! "$domain" =~ ^[a-z0-9][a-z0-9.-]*[a-z0-9]$ ]]; then
        echo "Error: Domain name must be a valid domain (lowercase, numbers, dots, hyphens)."
        exit 1
    fi
}

BUCKET_NAME=""
DOMAIN_NAME=""
AWS_REGION=""

while [[ $# -gt 0 ]]; do
    case $1 in
        -b|--bucket-name)
            BUCKET_NAME="$2"
            shift 2
            ;;
        -d|--domain-name)
            DOMAIN_NAME="$2"
            shift 2
            ;;
        -r|--region)
            AWS_REGION="$2"
            shift 2
            ;;
        -h|--help)
            usage
            ;;
        *)
            echo "Error: Unknown option $1"
            usage
            ;;
    esac
done

if [[ -z "$BUCKET_NAME" ]]; then
    echo "Error: Bucket name is required"
    usage
fi

if [[ -z "$DOMAIN_NAME" ]]; then
    echo "Error: Domain name is required"
    usage
fi

validate_bucket_name "$BUCKET_NAME"
validate_domain_name "$DOMAIN_NAME"

if [[ ! -f "$TEMPLATE_FILE" ]]; then
    echo "Error: Template file not found: $TEMPLATE_FILE"
    exit 1
fi

REGION_ARG=""
if [[ -n "$AWS_REGION" ]]; then
    REGION_ARG="--region $AWS_REGION"
fi

echo "Deploying CloudFormation stack..."
echo "  Stack Name:  $STACK_NAME"
echo "  Bucket Name: $BUCKET_NAME"
echo "  Domain Name: $DOMAIN_NAME"
echo "  Template:    $TEMPLATE_FILE"
if [[ -n "$AWS_REGION" ]]; then
    echo "  Region:      $AWS_REGION"
fi
echo ""
echo "NOTE: While the stack deploys, watch the AWS Console for the Route53 hosted zone."
echo "      As soon as it appears, add the NameServers output as NS records in CloudFlare."
echo "      CloudFormation will wait for ACM certificate validation before continuing."
echo ""

aws cloudformation deploy \
    $REGION_ARG \
    --template-file "$TEMPLATE_FILE" \
    --stack-name "$STACK_NAME" \
    --parameter-overrides \
        BucketName="$BUCKET_NAME" \
        DomainName="$DOMAIN_NAME" \
    --capabilities CAPABILITY_NAMED_IAM

echo ""
echo "Deployment complete!"
echo ""
echo "Stack Outputs:"
aws cloudformation describe-stacks \
    $REGION_ARG \
    --stack-name "$STACK_NAME" \
    --query 'Stacks[0].Outputs' \
    --output table
