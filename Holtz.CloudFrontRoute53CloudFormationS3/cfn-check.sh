#!/bin/bash

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
STACK_NAME="image-serving-stack"

usage() {
    echo "Usage: $0 [-r <aws-region>]"
    echo ""
    echo "Check CloudFormation stack status and verify image access"
    echo ""
    echo "Options:"
    echo "  -r, --region    AWS region (default: from AWS CLI config)"
    echo "  -h, --help      Show this help message"
    exit 1
}

AWS_REGION=""

while [[ $# -gt 0 ]]; do
    case $1 in
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

echo "=========================================="
echo "Checking Stack Status"
echo "=========================================="

STACK_STATUS=$(aws cloudformation describe-stacks \
    $REGION_ARG \
    --stack-name "$STACK_NAME" \
    --query 'Stacks[0].StackStatus' \
    --output text 2>/dev/null || echo "NOT_FOUND")

if [[ "$STACK_STATUS" == "NOT_FOUND" ]]; then
    echo "ERROR: Stack '$STACK_NAME' not found!"
    exit 1
fi

echo "Stack Status: $STACK_STATUS"

if [[ "$STACK_STATUS" != "CREATE_COMPLETE" ]] && [[ "$STACK_STATUS" != "UPDATE_COMPLETE" ]]; then
    echo "WARNING: Stack is not in a complete state"
fi

echo ""
echo "=========================================="
echo "Stack Outputs"
echo "=========================================="

aws cloudformation describe-stacks \
    $REGION_ARG \
    --stack-name "$STACK_NAME" \
    --query 'Stacks[0].Outputs' \
    --output table

BUCKET_NAME=$(aws cloudformation describe-stacks \
    $REGION_ARG \
    --stack-name "$STACK_NAME" \
    --query 'Stacks[0].Outputs[?OutputKey==`BucketName`].OutputValue' \
    --output text)

CLOUDFRONT_URL=$(aws cloudformation describe-stacks \
    $REGION_ARG \
    --stack-name "$STACK_NAME" \
    --query 'Stacks[0].Outputs[?OutputKey==`CloudFrontURL`].OutputValue' \
    --output text)

CUSTOM_DOMAIN_URL=$(aws cloudformation describe-stacks \
    $REGION_ARG \
    --stack-name "$STACK_NAME" \
    --query 'Stacks[0].Outputs[?OutputKey==`CustomDomainURL`].OutputValue' \
    --output text)

DISTRIBUTION_ID=$(aws cloudformation describe-stacks \
    $REGION_ARG \
    --stack-name "$STACK_NAME" \
    --query 'Stacks[0].Outputs[?OutputKey==`DistributionId`].OutputValue' \
    --output text)

echo ""
echo "=========================================="
echo "Checking S3 Bucket"
echo "=========================================="

echo "Bucket Name: $BUCKET_NAME"

echo ""
echo "Objects in bucket:"
aws s3 ls s3://$BUCKET_NAME/ $REGION_ARG || echo "Cannot list bucket (may be empty or access denied)"

echo ""
echo "=========================================="
echo "Checking CloudFront Distribution"
echo "=========================================="

echo "Distribution ID: $DISTRIBUTION_ID"

CF_STATUS=$(aws cloudfront get-distribution \
    --id "$DISTRIBUTION_ID" \
    --query 'Distribution.Status' \
    --output text)

echo "Distribution Status: $CF_STATUS"

echo ""
echo "=========================================="
echo "How to Access Your Images"
echo "=========================================="

echo ""
echo "1. Via CloudFront URL (default):"
echo "   $CLOUDFRONT_URL/test-to-speech.png"
echo ""
echo "2. Via Custom Domain:"
echo "   $CUSTOM_DOMAIN_URL/test-to-speech.png"
echo ""
echo "   (DNS must propagate - may take up to 48 hours)"

echo ""
echo "=========================================="
echo "Testing Image Access"
echo "=========================================="

echo ""
echo "Testing CloudFront URL..."
HTTP_CODE=$(curl -s -o /dev/null -w "%{http_code}" "$CLOUDFRONT_URL/test-to-speech.png" 2>/dev/null || echo "000")
echo "CloudFront URL HTTP Code: $HTTP_CODE"

if [[ "$HTTP_CODE" == "200" ]]; then
    echo "SUCCESS: Image accessible via CloudFront URL!"
elif [[ "$HTTP_CODE" == "403" ]]; then
    echo "ERROR: Access Denied - Check OAI and bucket policy"
elif [[ "$HTTP_CODE" == "404" ]]; then
    echo "ERROR: Image not found - Check if test-to-speech.png exists in S3"
elif [[ "$HTTP_CODE" == "000" ]]; then
    echo "ERROR: Could not connect - Check internet access"
else
    echo "ERROR: Unexpected HTTP code: $HTTP_CODE"
fi

echo ""
echo "=========================================="
echo "DNS Check (for custom domain)"
echo "=========================================="

if [[ -n "$CUSTOM_DOMAIN_URL" ]]; then
    DOMAIN=$(echo "$CUSTOM_DOMAIN_URL" | sed 's|https://||')
    echo "Resolving: $DOMAIN"
    NS_LOOKUP=$(nslookup "$DOMAIN" 2>/dev/null | grep -A5 "Name:" || echo "Could not resolve")
    echo "$NS_LOOKUP"
    
    echo ""
    echo "Testing Custom Domain..."
    HTTP_CODE=$(curl -s -o /dev/null -w "%{http_code}" "$CUSTOM_DOMAIN_URL/test-to-speech.png" --resolve "$DOMAIN:443:$(echo "$CLOUDFRONT_URL" | sed 's|https://||')" 2>/dev/null || echo "000")
    echo "Custom Domain HTTP Code: $HTTP_CODE"
fi

echo ""
echo "=========================================="
echo "Troubleshooting Tips"
echo "=========================================="

echo ""
echo "1. If CloudFront returns 403:"
echo "   - Verify bucket policy allows OAI"
echo "   - Wait 5-10 minutes for distribution to deploy"
echo ""
echo "2. If custom domain not working:"
echo "   - Check NS records point to Route53"
echo "   - Wait for DNS propagation (up to 48 hours)"
echo "   - Run: nslookup your-domain.com"
echo ""
echo "3. If image not found:"
echo "   - Check S3 bucket has the file"
echo "   - Verify filename is exactly: test-to-speech.png"
echo ""
echo "4. To invalidate CloudFront cache:"
echo "   aws cloudfront create-invalidation --distribution-id $DISTRIBUTION_ID --paths \"/*\""

echo ""
