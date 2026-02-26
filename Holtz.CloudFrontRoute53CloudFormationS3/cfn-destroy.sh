#!/bin/bash

set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
STACK_NAME="image-serving-stack"

usage() {
    echo "Usage: $0 [-b <bucket-name>] [-r <aws-region>]"
    echo ""
    echo "Delete CloudFormation stack for image serving"
    echo ""
    echo "Options:"
    echo "  -b, --bucket-name    S3 bucket name (required if bucket is not empty)"
    echo "  -r, --region         AWS region (default: from AWS CLI config)"
    echo "  -h, --help           Show this help message"
    echo ""
    echo "Examples:"
    echo "  $0                              # Delete stack (bucket must be empty)"
    echo "  $0 -b my-bucket                 # Empty bucket and delete stack"
    echo "  $0 -b my-bucket -r us-east-1    # Specify region"
    exit 1
}

BUCKET_NAME=""
AWS_REGION=""

while [[ $# -gt 0 ]]; do
    case $1 in
        -b|--bucket-name)
            BUCKET_NAME="$2"
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

REGION_ARG=""
if [[ -n "$AWS_REGION" ]]; then
    REGION_ARG="--region $AWS_REGION"
fi

echo "Deleting CloudFormation stack..."
echo "  Stack Name: $STACK_NAME"
if [[ -n "$AWS_REGION" ]]; then
    echo "  Region:     $AWS_REGION"
fi
echo ""

if [[ -n "$BUCKET_NAME" ]]; then
    echo "Emptying S3 bucket: $BUCKET_NAME"
    aws s3 rm s3://$BUCKET_NAME --recursive $REGION_ARG || true
    echo ""
fi

aws cloudformation delete-stack $REGION_ARG --stack-name "$STACK_NAME"

echo "Waiting for stack deletion to complete..."
aws cloudformation wait stack-delete-complete $REGION_ARG --stack-name "$STACK_NAME"

echo ""
echo "Stack deleted successfully!"
