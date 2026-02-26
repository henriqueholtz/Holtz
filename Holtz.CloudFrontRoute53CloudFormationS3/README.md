# CloudFront + S3 + Route53 Image Serving Stack

A CloudFormation template that creates a cost-optimized infrastructure for serving images securely via CloudFront with a private S3 bucket as the origin and Route53 for custom domain routing.

## Architecture

```
┌─────────────┐     HTTPS      ┌─────────────────┐     OAI      ┌─────────────┐
│             │ ────────────── │                 │ ──────────── │             │
│    User     │                │   CloudFront    │              │  S3 Bucket  │
│             │ ◄────────────── │   (CDN)        │ ◄─────────── │  (Private)  │
└─────────────┘     Image      └────────┬────────┘     File    └─────────────┘
                                         │
                                         │ Alias Record
                                         ▼
                                ┌─────────────────┐
                                │     Route53     │
                                │  (Hosted Zone)  │
                                └─────────────────┘
```

### Full DNS Flow (with CloudFlare)

```mermaid
flowchart LR
    User[User] -->|HTTPS Request| CloudFlare[CloudFlare DNS]
    CloudFlare -->|NS Records| Route53[Route53 Hosted Zone]
    Route53 -->|A Alias Record| CloudFront[CloudFront CDN]
    CloudFront -->|OAI Request| S3[S3 Bucket\nPrivate]
    
    subgraph "AWS"
    Route53
    CloudFront
    S3
    end
    
    subgraph "CloudFlare"
    CloudFlare
    end
```

## Resources Created

| Resource | Type | Description |
|----------|------|-------------|
| ImageBucket | AWS::S3::Bucket | Private S3 bucket for image storage |
| ImageBucketPolicy | AWS::S3::BucketPolicy | Allows CloudFront OAI to read objects |
| CloudFrontOAI | AWS::CloudFront::CloudFrontOriginAccessIdentity | Secure access identity |
| CloudFrontDistribution | AWS::CloudFront::Distribution | CDN with HTTPS enabled |
| Route53Record | AWS::Route53::RecordSet | Alias A record pointing to CloudFront |

## Cost Estimate

### Free Tier (First 12 Months)

| Service | Free Tier | Monthly Usage | Cost |
|---------|-----------|---------------|------|
| S3 Storage | 5 GB | ~1 GB | **$0.00** |
| S3 PUT Requests | 2,000 | ~100 uploads | **$0.00** |
| S3 GET Requests | 20,000 | ~10,000 views | **$0.00** |
| CloudFront Transfer | 1 TB | ~10 GB | **$0.00** |
| CloudFront Requests | 10 Million | ~100,000 | **$0.00** |
| Route53 Hosted Zone | - | 1 zone | **$0.50** |
| **Total** | | | **~$0.50/month** |

### After Free Tier (Estimated)

| Service | Pricing | Monthly Usage | Cost |
|---------|---------|---------------|------|
| S3 Storage | $0.023/GB | 1 GB | **$0.02** |
| S3 PUT Requests | $0.005/1,000 | 100 | **$0.001** |
| S3 GET Requests | $0.0004/1,000 | 10,000 | **$0.004** |
| CloudFront Transfer | $0.085/GB (US) | 10 GB | **$0.85** |
| CloudFront Requests | $0.0075/10,000 | 100,000 | **$0.075** |
| Route53 Hosted Zone | $0.50/zone | 1 zone | **$0.50** |
| **Total** | | | **~$1.45/month** |

## Prerequisites

- AWS CLI installed and configured (`aws configure`)
- An AWS account with appropriate permissions
- A globally unique S3 bucket name
- An existing Route53 **public** Hosted Zone for your domain
- An ACM Certificate in **us-east-1** for your custom domain (must be validated)

## Complete Setup Guide (CloudFlare + Route53)

This guide covers the full setup when your domain is managed by CloudFlare but you want to use Route53 for DNS.

### Step 1: Request ACM Certificate

```bash
# Request certificate in us-east-1 (required for CloudFront)
aws acm request-certificate \
  --domain-name your-domain.com \
  --subject-alternative-names "*.your-domain.com" \
  --validation-method DNS \
  --region us-east-1
```

Note the Certificate ARN from the output.

### Step 2: Get ACM Validation Records

```bash
aws acm describe-certificate \
  --certificate-arn arn:aws:acm:us-east-1:YOUR_ACCOUNT:certificate/YOUR_CERT_ID \
  --region us-east-1 \
  --query 'Certificate.DomainValidationOptions'
```

This returns CNAME name and value for validation.

### Step 3: Add ACM Validation to CloudFlare

1. Go to CloudFlare → your-domain.com → DNS
2. Add a CNAME record:
   - **Name**: `_your-acm-cname-name` (from Step 2)
   - **Value**: `_your-acm-cname-value.acm-validations.aws.`
   - **Proxy status**: DNS only (gray cloud)
3. Wait 5-10 minutes

### Step 4: Verify ACM Certificate Status

```bash
aws acm describe-certificate \
  --certificate-arn arn:aws:acm:us-east-1:YOUR_ACCOUNT:certificate/YOUR_CERT_ID \
  --region us-east-1 \
  --query 'Certificate.Status'
```

Must return `ISSUED` before proceeding.

### Step 5: Create Route53 Hosted Zone

```bash
aws route53 create-hosted-zone \
  --name your-domain.com \
  --caller-reference "your-domain-$(date +%s)"
```

Note the NS records from the output.

### Step 6: Update CloudFlare NS Records

In CloudFlare (for your parent domain):
1. Go to DNS settings
2. Update NS records to point to Route53 nameservers:
   - `ns-xxx.awsdns-y.com`
   - `ns-xxx.awsdns-y.net`
   - `ns-xxx.awsdns-y.org`
   - `ns-xxx.awsdns-y.co.uk`

### Step 7: Get Hosted Zone ID

```bash
aws route53 list-hosted-zones \
  --query 'HostedZones[?Name==`your-domain.com.`].Id' \
  --output text
```

Returns `/hostedzone/Z1234567890ABC` (use only `Z1234567890ABC`).

---

### Automated: Deploy with cfn-deploy.sh

```bash
chmod +x cfn-deploy.sh

./cfn-deploy.sh \
  -b your-unique-bucket-name \
  -d your-domain.com \
  -z Z1234567890ABC \
  -c arn:aws:acm:us-east-1:YOUR_ACCOUNT:certificate/YOUR_CERT_ID
```

This creates: S3 bucket, CloudFront distribution, Route53 A record.

---

## Usage

### Upload an Image

```bash
# Upload to S3
aws s3 cp path/to/image.jpg s3://your-unique-bucket-name/

# Upload with custom key (folder-like structure)
aws s3 cp path/to/image.jpg s3://your-unique-bucket-name/products/product-123.jpg
```

### Access via Custom Domain

```
https://images.example.com/image.jpg
https://images.example.com/products/product-123.jpg
```

### Upload Multiple Images

```bash
# Sync entire directory
aws s3 sync ./images/ s3://your-unique-bucket-name/
```

## CloudFront Configuration

| Setting | Value | Reason |
|---------|-------|--------|
| Price Class | PriceClass_100 | US/Canada/Europe only (cheapest) |
| Viewer Protocol | Redirect to HTTPS | Security + SEO |
| HTTP Version | HTTP/2 | Performance |
| Compression | Enabled | Faster transfer |
| Cache Policy | CachingOptimized | 1 year TTL, reduces S3 costs |
| SSL Certificate | ACM Certificate | Custom domain HTTPS |

## Security Features

- **Private S3 Bucket**: No public access, all public access blocks enabled
- **Origin Access Identity (OAI)**: Only CloudFront can access S3 objects
- **Server-Side Encryption**: AES-256 encryption at rest
- **HTTPS Enforcement**: All traffic redirected to HTTPS
- **TLS 1.2 Minimum**: Modern SSL/TLS security

## Cleanup

### Automated: Delete Stack with cfn-destroy.sh

```bash
chmod +x cfn-destroy.sh

# Delete stack (bucket must be empty)
./cfn-destroy.sh

# Empty bucket and delete stack
./cfn-destroy.sh -b your-unique-bucket-name

# Optional: specify AWS region
./cfn-destroy.sh -b your-unique-bucket-name -r us-east-1
```

### Manual: Complete Removal

If you want to remove everything completely:

1. **Delete CloudFormation stack** (see above)

2. **Delete Route53 Hosted Zone** (if created for this project):
   ```bash
   aws route53 delete-hosted-zone --id /hostedzone/Z1234567890ABC
   ```

3. **Delete ACM Certificate**:
   ```bash
   aws acm delete-certificate \
     --certificate-arn arn:aws:acm:us-east-1:YOUR_ACCOUNT:certificate/YOUR_CERT_ID \
     --region us-east-1
   ```

4. **Restore CloudFlare NS Records** (if you changed them):
   - Go to CloudFlare → DNS
   - Update NS records back to CloudFlare nameservers
   - Remove ACM validation CNAME record

## Troubleshooting

### Quick Check with cfn-check.sh

Run the check script to diagnose common issues:

```bash
chmod +x cfn-check.sh
./cfn-check.sh
```

This script checks:
- Stack status (CREATE_COMPLETE, etc.)
- S3 bucket and objects
- CloudFront distribution status
- Image accessibility via CloudFront URL
- DNS resolution for custom domain

### Common Issues

#### 403 Forbidden Error

1. Verify bucket policy is applied correctly
2. Ensure OAI is properly configured
3. Wait 5-10 minutes for CloudFront propagation

### CloudFront Not Serving New Content

1. Invalidate the cache:
   ```bash
   aws cloudfront create-invalidation \
     --distribution-id YOUR_DISTRIBUTION_ID \
     --paths "/*"
   ```

### Certificate Validation Failed

1. Ensure ACM certificate is in us-east-1 region
2. Verify DNS validation records are added to Route53
3. Wait for validation to complete (can take several minutes)

### Domain Not Resolving

1. Verify Route53 record was created correctly
2. Check that nameservers match your domain registrar settings
3. DNS propagation can take up to 48 hours (usually much faster)

### Bucket Name Already Exists

S3 bucket names are globally unique. Use a unique name with your account ID:
```bash
./cfn-deploy.sh -b my-images-$(aws sts get-caller-identity --query Account --output text) ...
```

## File Structure

```
.
├── README.md
├── cfn-deploy.sh
├── cfn-check.sh
├── cfn-destroy.sh
└── cloudformation/
    └── stack.yaml
```

## License

MIT
