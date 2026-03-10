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
| HostedZone | AWS::Route53::HostedZone | Public hosted zone for the custom domain |
| AcmCertificate | AWS::CertificateManager::Certificate | SSL/TLS certificate (DNS-validated via Route53) |
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
- A domain managed in CloudFlare

## Setup Guide (CloudFlare + Route53)

The CloudFormation stack automatically creates the Route53 hosted zone and ACM certificate. The only manual step is adding NS delegation records in CloudFlare.

> **Important:** This stack must be deployed in **us-east-1** — ACM certificates for CloudFront must be in that region.

### Step 1: Deploy the stack

```bash
chmod +x cfn-deploy.sh

./cfn-deploy.sh \
  -b your-unique-bucket-name \
  -d images.example.com \
  -r us-east-1
```

This creates: Route53 hosted zone, ACM certificate, S3 bucket, CloudFront distribution, and Route53 A alias record.

### Step 2: Add NS records in CloudFlare

While the stack is deploying (it will pause waiting for ACM certificate validation), watch the AWS Console for the Route53 hosted zone to be created. Then:

1. Run the check script or look at the CloudFormation stack events in the console to get the nameservers:
   ```bash
   ./cfn-check.sh -r us-east-1
   ```
2. Go to **CloudFlare → your domain → DNS**
3. Add 4 **NS records** for your subdomain (e.g., `images.example.com`) pointing to the Route53 nameservers from the stack output:
   - **Type**: NS
   - **Name**: `images` (or the subdomain part)
   - **Value**: `ns-xxx.awsdns-y.com` (one record per nameserver)
   - **Proxy status**: DNS only (gray cloud)

Once DNS propagates, ACM validates automatically and the stack finishes deploying.

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

1. **Delete CloudFormation stack** (see above) — this also deletes the Route53 hosted zone and ACM certificate automatically.

2. **Remove NS delegation from CloudFlare**:
   - Go to CloudFlare → DNS
   - Remove the NS records you added for the subdomain

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

### Stack Stuck Waiting for Certificate Validation

CloudFormation automatically creates the ACM DNS validation record in Route53, but ACM cannot validate until Route53 is authoritative for the domain. Fix:

1. Get the Route53 nameservers from the check script or the AWS Console (CloudFormation → stack events)
2. Add NS delegation records for your subdomain in CloudFlare (DNS only, gray cloud)
3. Wait for DNS propagation (usually a few minutes)
4. ACM will validate automatically and the stack will continue

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
