#!/bin/bash

set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
TEMPLATE_FILE="${SCRIPT_DIR}/cloudformation/stack.yaml"
STACK_NAME="image-serving-stack"

usage() {
    echo "Usage: $0 -b <bucket-name> -d <domain-name> -z <hosted-zone-id> -c <certificate-arn> [-r <aws-region>]"
    echo ""
    echo "Deploy CloudFormation stack for image serving with S3 + CloudFront + Route53"
    echo ""
    echo "Options:"
    echo "  -b, --bucket-name      S3 bucket name (required, must be globally unique)"
    echo "  -d, --domain-name      Custom domain name (required, e.g., images.example.com)"
    echo "  -z, --hosted-zone-id   Route53 Hosted Zone ID (required)"
    echo "  -c, --certificate-arn  ACM Certificate ARN (required, must be in us-east-1)"
    echo "  -r, --region           AWS region (default: from AWS CLI config)"
    echo "  -h, --help             Show this help message"
    echo ""
    echo "Example:"
    echo "  $0 -b my-images-bucket -d images.example.com -z Z1234567890ABC -c arn:aws:acm:us-east-1:123456789012:certificate/abc123"
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
HOSTED_ZONE_ID=""
CERTIFICATE_ARN=""
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
        -z|--hosted-zone-id)
            HOSTED_ZONE_ID="$2"
            shift 2
            ;;
        -c|--certificate-arn)
            CERTIFICATE_ARN="$2"
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

if [[ -z "$HOSTED_ZONE_ID" ]]; then
    echo "Error: Hosted Zone ID is required"
    usage
fi

if [[ -z "$CERTIFICATE_ARN" ]]; then
    echo "Error: ACM Certificate ARN is required"
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
echo "  Stack Name:       $STACK_NAME"
echo "  Bucket Name:      $BUCKET_NAME"
echo "  Domain Name:      $DOMAIN_NAME"
echo "  Hosted Zone ID:   $HOSTED_ZONE_ID"
echo "  Certificate ARN:  $CERTIFICATE_ARN"
echo "  Template:         $TEMPLATE_FILE"
if [[ -n "$AWS_REGION" ]]; then
    echo "  Region:           $AWS_REGION"
fi
echo ""

aws cloudformation deploy \
    $REGION_ARG \
    --template-file "$TEMPLATE_FILE" \
    --stack-name "$STACK_NAME" \
    --parameter-overrides \
        BucketName="$BUCKET_NAME" \
        DomainName="$DOMAIN_NAME" \
        HostedZoneId="$HOSTED_ZONE_ID" \
        AcmCertificateArn="$CERTIFICATE_ARN" \
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
