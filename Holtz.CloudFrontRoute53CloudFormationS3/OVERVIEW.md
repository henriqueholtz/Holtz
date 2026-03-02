# AWS CloudFront + S3 + Route53 Architecture Overview

This document provides a comprehensive technical overview of the CloudFront-S3-Route53 infrastructure stack deployed in this repository. It is designed for senior software developers preparing for the AWS Solutions Architect Associate certification.

---

## Table of Contents

1. [Architecture Overview](#architecture-overview)
2. [AWS Services Deep Dive](#aws-services-deep-dive)
   - [Amazon S3](#amazon-s3)
   - [Amazon CloudFront](#amazon-cloudfront)
   - [Amazon Route 53](#amazon-route-53)
   - [AWS CloudFormation](#aws-cloudformation)
   - [AWS Certificate Manager](#aws-certificate-manager)
3. [DNS Resolution Flow](#dns-resolution-flow)
4. [Security Architecture](#security-architecture)
5. [Data Flow and Request Path](#data-flow-and-request-path)
6. [CloudFormation Template Analysis](#cloudformation-template-analysis)
7. [Certification-Relevant Concepts](#certification-relevant-concepts)
8. [Current Deployment Status](#current-deployment-status)
9. [Troubleshooting Common Issues](#troubleshooting-common-issues)
10. [Cost Considerations](#cost-considerations)
11. [Best Practices](#best-practices)

---

## Architecture Overview

This infrastructure implements a secure, cost-optimized content delivery network (CDN) for serving static images. The architecture follows AWS best practices for security, scalability, and cost efficiency.

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                              INTERNET                                        │
│                                                                             │
│   ┌──────────────┐                      ┌──────────────────────────────┐  │
│   │   Browser /  │   HTTPS Request      │                              │  │
│   │   Client     │ ───────────────────► │    Route53 DNS Service      │  │
│   │              │                       │    (Authoritative DNS)      │  │
│   └──────────────┘                       └──────────────┬───────────────┘  │
│                                                        │                    │
│                                                        │ A Record           │
│                                                        ▼                    │
│   ┌─────────────────────────────────────────────────────────────────────┐   │
│   │                        AWS CLOUD                                   │   │
│   │                                                                       │   │
│   │   ┌─────────────────┐        ┌─────────────────────────────────┐   │   │
│   │   │  CloudFront     │        │       S3 Bucket (Private)       │   │   │
│   │   │  Distribution   │───────►│                                 │   │   │
│   │   │  (CDN Edge     │        │   ┌───────────────────────────┐   │   │   │
│   │   │   Locations)  │        │   │  Image Files (Private)   │   │   │   │
│   │   │                │        │   │  - Encrypted at Rest    │   │   │   │
│   │   │  ┌──────────┐  │        │   │  - AES-256 SSE         │   │   │   │
│   │   │  │   OAI    │──┼────────┼──►│                         │   │   │   │
│   │   │  │(Identity)│  │        │   └───────────────────────────┘   │   │   │
│   │   │  └──────────┘  │        │                                 │   │   │
│   │   └─────────────────┘        └─────────────────────────────────┘   │   │
│   │                                                                       │   │
│   └─────────────────────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────────────────────┘

DNS: test.talkient.app ──► CloudFront Domain ──► S3 Bucket
```

### Deployment Parameters

| Parameter | Value | Description |
|-----------|-------|-------------|
| BucketName | henriqueholtz-cloudfront-test | Globally unique S3 bucket name |
| DomainName | test.talkient.app | Custom domain for CloudFront |
| HostedZoneId | (from Route53) | Route53 Hosted Zone ID |
| AcmCertificateArn | (from ACM) | SSL certificate for HTTPS |

---

## AWS Services Deep Dive

### Amazon S3

**Service Overview:** Amazon Simple Storage Service (S3) provides object storage with built-in durability, availability, and scalability.

#### S3 Bucket Configuration in This Stack

```yaml
ImageBucket:
  Type: AWS::S3::Bucket
  Properties:
    BucketName: !Ref BucketName
    PublicAccessBlockConfiguration:
      BlockPublicAcls: true
      BlockPublicPolicy: true
      IgnorePublicAcls: true
      RestrictPublicBuckets: true
    BucketEncryption:
      ServerSideEncryptionConfiguration:
        - ServerSideEncryptionByDefault:
            SSEAlgorithm: AES256
```

#### Key S3 Concepts for Certification

| Feature | Description | Exam Relevance |
|---------|-------------|----------------|
| **Buckets** | Containers for objects; globally unique names | Must know naming rules |
| **Objects** | Files stored in buckets; consist of data + metadata | Key-value architecture |
| **Regions** | S3 stores data in specified region | Data residency implications |
| **Durability** | 99.999999999% (11 9's) designed durability | Design for failure |
| **Availability** | 99.99% uptime SLA | Understand the difference from durability |

#### S3 Storage Classes (Relevant for Image Hosting)

| Class | Use Case | Retrieval Fee |
|-------|----------|----------------|
| **S3 Standard** | Frequently accessed data | None |
| S3 Intelligent-Tiering | Unknown access patterns | Small monitoring fee |
| S3 Standard-IA | Infrequently accessed | Yes |
| S3 Glacier | Archival | Yes |

**This stack uses S3 Standard** (default) because images are served via CloudFront and cached at edge locations.

#### S3 Security Features

1. **Block Public Access**: Prevents accidental public exposure
   - `BlockPublicAcls`: Blocks ACL-based access
   - `BlockPublicPolicy`: Blocks bucket policies that grant public access
   - `IgnorePublicAcls`: Ignores external ACLs
   - `RestrictPublicBuckets`: Blocks cross-account access via bucket policy

2. **Server-Side Encryption (SSE)**
   - **SSE-S3**: AWS manages keys, AES-256 (used in this stack)
   - **SSE-KMS**: AWS Key Management Service with customer-managed keys
   - **SSE-C**: Customer-provided encryption keys

3. **S3 Bucket Policy**: Resource-based IAM policy for access control

#### S3 Pricing Components

| Component | Price (US East) | Notes |
|-----------|-----------------|-------|
| Storage | $0.023/GB/month | First 50TB |
| PUT/COPY | $0.005/1,000 requests | Upload operations |
| GET/SELECT | $0.0004/1,000 requests | Download operations |
| Data Transfer | $0.09/GB out to internet | After free tier |

---

### Amazon CloudFront

**Service Overview:** Amazon CloudFront is a content delivery network (CDN) that delivers data, videos, applications, and APIs to customers globally with low latency and high transfer speeds.

#### CloudFront Distribution Configuration

```yaml
CloudFrontDistribution:
  Type: AWS::CloudFront::Distribution
  Properties:
    DistributionConfig:
      Enabled: true
      Aliases:
        - !Ref DomainName
      DefaultRootObject: index.html
      PriceClass: PriceClass_100
      HttpVersion: http2
      ViewerCertificate:
        AcmCertificateArn: !Ref AcmCertificateArn
        SslSupportMethod: sni-only
        MinimumProtocolVersion: TLSv1.2_2021
      DefaultCacheBehavior:
        TargetOriginId: S3Origin
        ViewerProtocolPolicy: redirect-to-https
        AllowedMethods:
          - GET
          - HEAD
          - OPTIONS
        CachedMethods:
          - GET
          - HEAD
        CachePolicyId: 658327ea-f89d-4fab-a63d-7e88639e58f6
        Compress: true
      Origins:
        - DomainName: !GetAtt ImageBucket.RegionalDomainName
          Id: S3Origin
          S3OriginConfig:
            OriginAccessIdentity: !Sub 'origin-access-identity/cloudfront/${CloudFrontOAI}'
```

#### Key CloudFront Concepts for Certification

| Concept | Description | Exam Relevance |
|---------|-------------|----------------|
| **Edge Locations** | 450+ global points of presence | Understand caching |
| **Regional Edge Caches** | 13 regional caches | Latency optimization |
| **Origins** | Where CloudFront fetches content | S3, ALB, custom HTTP |
| **Cache Behaviors** | Path-specific caching rules | Pattern matching |
| **Invalidations** | Clear cached content | Different from versioning |

#### CloudFront Price Classes

| Class | Regions Covered | Cost Savings |
|-------|-----------------|--------------|
| **PriceClass_100** | US, Canada, Europe | ~70% of default |
| PriceClass_200 | + South America, Middle East, Africa | ~90% of default |
| **PriceClass_All** | Global | Full price |

**This stack uses PriceClass_100** for cost optimization (US/Canada/Europe only).

#### CloudFront SSL/TLS Options

| Option | Description | Use Case |
|--------|-------------|----------|
| **Default CloudFront Certificate** | *.cloudfront.net domain | Simple setups |
| **Custom SSL (ACM)** | Your own domain with ACM cert | Custom domains (used) |
| **Custom SSL (IAM)** | Upload your own certificate | Legacy support |

#### Origin Access Identity (OAI)

OAI is a special CloudFront user that provides secure access to private S3 content:

```
┌─────────────────────────────────────────────────────────────────┐
│                    WITHOUT OAI (Insecure)                       │
│                                                                  │
│   User ──────► CloudFront ──────► S3 Bucket (Public Access)    │
│                                                                  │
│   ✗ Anyone can access S3 directly                               │
│   ✗ No protection if CloudFront is compromised                 │
└─────────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────────┐
│                    WITH OAI (Secure - This Stack)               │
│                                                                  │
│   User ──────► CloudFront ──────► S3 Bucket (Private)          │
│                            │                                     │
│                            ▼                                     │
│                    OAI Identity ──► Access Granted              │
│                                                                  │
│   ✓ Only CloudFront can access S3                              │
│   ✓ S3 bucket is completely private                             │
│   ✓ Signed URLs for user-specific access possible              │
└─────────────────────────────────────────────────────────────────┘
```

#### Cache Policy (CachingOptimized)

The stack uses the managed cache policy `658327ea-f89d-4fab-a63d-7e88639e58f6` (CachingOptimized):

| Setting | Value | Impact |
|---------|-------|--------|
| Minimum TTL | 1 second | Allows quick invalidation |
| Maximum TTL | 31536000 seconds (1 year) | Long-term caching |
| Default TTL | 86400 seconds (1 day) | Baseline caching |

#### CloudFront Request Flow

```
1. User requests: https://test.talkient.app/image.png

2. DNS Resolution (Route53):
   test.talkient.app ──► CloudFront Distribution Domain Name

3. Edge Location Processing:
   ┌──────────────────────────────────────────────┐
   │  Edge Location                               │
   │  ┌────────────────────────────────────────┐  │
   │  │ Check: Is object in cache?             │  │
   │  └────────────────────────────────────────┘  │
   │         │                                     │
   │    YES  │  NO                                 │
   │         ▼                                     │
   │  ┌─────────────┐                              │
   │  │ Serve from  │                              │
   │  │ Cache       │                              │
   │  └─────────────┘                              │
   │                                              │
   │         │                                     │
   │         ▼                                     │
   │  ┌─────────────┐    ┌───────────────────┐    │
   │  │ Origin      │───►│ S3 Bucket (via    │    │
   │  │ Request     │    │ OAI)              │    │
   │  └─────────────┘    └───────────────────┘    │
   │         │                                     │
   │         ▼                                     │
   │  ┌─────────────┐                              │
   │  │ Cache and   │                              │
   │  │ Return to   │                              │
   │  │ User        │                              │
   │  └─────────────┘                              │
   └──────────────────────────────────────────────┘
```

---

### Amazon Route 53

**Service Overview:** Amazon Route 53 is a scalable DNS web service that provides reliable routing to infrastructure running in AWS.

#### Route 53 Record Configuration

```yaml
Route53Record:
  Type: AWS::Route53::RecordSet
  Properties:
    HostedZoneId: !Ref HostedZoneId
    Name: !Ref DomainName
    Type: A
    AliasTarget:
      HostedZoneId: Z2FDTNDATAQYW2
      DNSName: !GetAtt CloudFrontDistribution.DomainName
      EvaluateTargetHealth: false
```

#### Key Route 53 Concepts for Certification

| Concept | Description | Exam Relevance |
|---------|-------------|----------------|
| **Hosted Zones** | Container for DNS records | Fundamental unit |
| **Record Sets** | Individual DNS records | A, AAAA, CNAME, etc. |
| **Alias Records** | AWS-specific records pointing to AWS resources | Free of charge |
| **Routing Policies** | Simple, weighted, latency, geolocation, failover | Design scenarios |
| **Health Checks** | Monitor endpoint health | Failover scenarios |

#### DNS Record Types Used

| Type | Purpose | Example |
|------|---------|---------|
| **A** | IPv4 address mapping | test.talkient.app → 52.84.x.x |
| **AAAA** | IPv6 address mapping | (not used) |
| **CNAME** | Canonical name | subdomain → another.domain.com |
| **NS** | Name server delegation | talkient.app → ns1.awsdns.com |
| **SOA** | Start of authority | Zone metadata |
| **TXT** | Text records | SPF, DKIM, verification |

#### Alias Records vs CNAME Records

| Feature | Alias Record | CNAME Record |
|---------|--------------|--------------|
| **Cost** | Free | Standard DNS pricing |
| **Root domain** | ✓ Supported | ✗ Not recommended |
| **Subdomains** | ✓ Supported | ✓ Supported |
| **Native AWS integration** | ✓ Yes | ✗ No |
| **Resolution** | AWS handles internally | External lookup |

**This stack uses Alias A record** pointing to CloudFront (no CNAME resolution needed, free).

#### Route 53 Hosted Zone ID for CloudFront

The special hosted zone ID `Z2FDTNDATAQYW2` is used when creating alias records to CloudFront:

```python
# This is a MAGIC VALUE - always the same for CloudFront
HostedZoneId: Z2FDTNDATAQYW2  # CloudFront's hosted zone ID
```

This is one of the few hardcoded AWS values that never changes.

#### Routing Policies

This stack uses **Simple Routing Policy** (default). Other options:

| Policy | Use Case | Example |
|--------|----------|---------|
| **Simple** | Single resource | (this stack) |
| Weighted | Split traffic | 80% → v1, 20% → v2 |
| Latency | Lowest latency | Route to nearest region |
| Geolocation | By user location | Different content per country |
| Failover | Health check based | Primary → secondary |
| Multi-value | Multiple healthy answers | DNS load balancing |

---

### AWS CloudFormation

**Service Overview:** AWS CloudFormation provides infrastructure as code (IaC), enabling you to provision and manage AWS resources in a repeatable, automated manner.

#### CloudFormation Template Structure

```yaml
AWSTemplateFormatVersion: '2010-09-09'
Description: S3 bucket with CloudFront distribution and Route53 for secure image serving

Parameters:           # Input values (customizable at deployment)
  - BucketName
  - DomainName
  - HostedZoneId
  - AcmCertificateArn

Resources:            # AWS resources to create
  - ImageBucket
  - ImageBucketPolicy
  - CloudFrontOAI
  - CloudFrontDistribution
  - Route53Record

Outputs:              # Return values after stack creation
  - BucketName
  - CloudFrontURL
  - CustomDomainURL
  - DistributionId
```

#### CloudFormation Intrinsic Functions Used

| Function | Syntax | Purpose |
|----------|--------|---------|
| `Ref` | `!Ref BucketName` | Returns resource name or parameter value |
| `Sub` | `!Sub '${Var}'` | String substitution |
| `GetAtt` | `!GetAtt Bucket.RegionalDomainName` | Returns attribute value |

#### CloudFormation Benefits for Certification

1. **Declarative**: Define desired state, AWS handles ordering
2. **Idempotent**: Safe to run multiple times
3. **Version Control**: Infrastructure code in Git
4. **Change Sets**: Preview changes before applying
5. **Rollback**: Automatic on failure
6. **Nested Stacks**: Modular, reusable templates

#### Stack States

| State | Description |
|-------|-------------|
| CREATE_IN_PROGRESS | Resources being created |
| CREATE_COMPLETE | Successfully created |
| CREATE_FAILED | Error during creation |
| UPDATE_IN_PROGRESS | Updating resources |
| UPDATE_COMPLETE | Update successful |
| UPDATE_ROLLBACK_IN_PROGRESS | Reverting changes |
| DELETE_IN_PROGRESS | Removing resources |
| DELETE_COMPLETE | Successfully deleted |
| DELETE_FAILED | Could not delete |

---

### AWS Certificate Manager (ACM)

**Service Overview:** AWS Certificate Manager provisions, manages, and deploys SSL/TLS certificates for AWS services.

#### ACM Certificate Configuration

```yaml
ViewerCertificate:
  AcmCertificateArn: !Ref AcmCertificateArn
  SslSupportMethod: sni-only
  MinimumProtocolVersion: TLSv1.2_2021
```

#### Key ACM Concepts for Certification

| Concept | Description |
|---------|-------------|
| **Regional Service** | ACM certificates are regional |
| **us-east-1 Requirement** | Must be in us-east-1 for CloudFront |
| **DNS Validation** | Automatic certificate renewal |
| **Email Validation** | Manual approval |
| **Import Certificates** | Use existing third-party certs |

#### SSL/TLS Protocol Versions

| Version | Status | Notes |
|---------|--------|-------|
| SSL 3.0 | ✗ Deprecated | POODLE vulnerability |
| TLS 1.0 | ✗ Deprecated | Vulnerabilities |
| TLS 1.1 | ✗ Deprecated | Vulnerabilities |
| TLS 1.2 | ✓ Current | Minimum for this stack |
| TLS 1.3 | ✓ Latest | Improved performance |

#### SSL Support Methods

| Method | Description | Browser Support |
|--------|-------------|-----------------|
| **sni-only** | Server Name Indication | Modern browsers (used) |
| vip | Legacy dedicated IP | Older browsers |

---

## DNS Resolution Flow

### Complete DNS Query Path

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                     DNS RESOLUTION FLOW                                      │
└─────────────────────────────────────────────────────────────────────────────┘

1. USER TYPES URL
   https://test.talkient.app/test-to-speech.png

2. LOCAL DNS RESOLVER
   │
   ▼ Queries for: test.talkient.app

3. ROUTE53 HOSTED ZONE (Authoritative DNS)
   │
   ├── Looks up: test.talkient.app
   ├── Finds: A Record (Alias) → d2lg1os7p6l8sn.cloudfront.net
   │
   ▼ Returns: d2lg1os7p6l8sn.cloudfront.net

4. RESOLVER → CLOUDFRONT EDGE LOCATION
   │
   ├── User's DNS resolves to nearest edge location
   ├── Edge location IP returned
   │
   ▼

5. CLOUDFRONT EDGE LOCATION
   │
   ├── Checks cache for: /test-to-speech.png
   │   ├── HIT: Return cached object
   │   │
   │   └── MISS: Request from S3 origin (via OAI)
   │       │
   │       ├── S3 validates OAI permissions
   │       ├── S3 returns object
   │       │
   │       ▼
   │       CloudFront caches object at edge
   │
   ▼ Returns: Image data (200 OK)

6. USER RECEIVES IMAGE
```

### DNS Record Hierarchy (From talkient.app.txt)

```
talkient.app (Parent Domain - CloudFlare Managed)
│
├── NS Records: karl.ns.cloudflare.com, luciane.ns.cloudflare.com
│
├── test.talkient.app (Subdomain - Route53 Managed via CloudFlare)
│   ├── NS Records: ns-1525.awsdns-62.org, ns-440.awsdns-55.com, ns-708.awsdns-24.net, ns-1610.awsdns-09.co.uk
│   └── A Record (Alias): test.talkient.app → CloudFront Distribution
│
├── api.talkient.app
│   └── A Record: 54.158.28.133 (AWS Lightsail)
│
├── talkient.app
│   ├── CNAME: talkient-landingpage.pages.dev (CloudFlare)
│   ├── TXT: SPF record
│   ├── TXT: DKIM record
│   └── MX Records: CloudFlare email
│
└── www.talkient.app
    └── CNAME: talkient-landingpage.pages.dev
```

**DNS Delegation Setup**

For subdomains managed by Route53 while parent domain is on CloudFlare, NS records must be added in CloudFlare:

| Type | Name | Value |
|------|------|-------|
| NS | test | ns-1525.awsdns-62.org |
| NS | test | ns-440.awsdns-55.com |
| NS | test | ns-708.awsdns-24.net |
| NS | test | ns-1610.awsdns-09.co.uk |

This delegates DNS resolution for `test.talkient.app` to AWS Route53.

---

## Security Architecture

### Defense in Depth

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                         SECURITY LAYERS                                      │
└─────────────────────────────────────────────────────────────────────────────┘

LAYER 1: Network Security
─────────────────────────
┌─────────────────────────────────────────┐
│  Route53                                │
│  • Private hosted zone option          │
│  • DNSSEC signing (optional)            │
└─────────────────────────────────────────┘
         │
         ▼
LAYER 2: Transport Security
──────────────────────────
┌─────────────────────────────────────────┐
│  CloudFront                             │
│  • HTTPS only (redirect-to-https)       │
│  • TLS 1.2 minimum                      │
│  • SNI-only (modern ciphers)            │
└─────────────────────────────────────────┘
         │
         ▼
LAYER 3: Access Control
───────────────────────
┌─────────────────────────────────────────┐
│  OAI + S3 Bucket Policy                 │
│  • Only CloudFront can access S3        │
│  • No direct S3 access                  │
│  • Principal: cloudfront:user/OAI      │
└─────────────────────────────────────────┘
         │
         ▼
LAYER 4: Data Protection
────────────────────────
┌─────────────────────────────────────────┐
│  S3 Encryption                          │
│  • AES-256 at rest                      │
│  • Server-side encryption               │
│  • KMS key management (optional)        │
└─────────────────────────────────────────┘
         │
         ▼
LAYER 5: S3 Block Public Access
──────────────────────────────
┌─────────────────────────────────────────┐
│  BlockPublicAcls: true                  │
│  BlockPublicPolicy: true                │
│  IgnorePublicAcls: true                 │
│  RestrictPublicBuckets: true            │
└─────────────────────────────────────────┘
```

### S3 Bucket Policy Analysis

```json
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Sid": "AllowCloudFrontRead",
      "Effect": "Allow",
      "Principal": {
        "AWS": "arn:aws:iam::cloudfront:user/CloudFront Origin Access Identity E1MVPTRTOI863N"
      },
      "Action": "s3:GetObject",
      "Resource": "arn:aws:s3:::henriqueholtz-cloudfront-test/*"
    }
  ]
}
```

**Policy Analysis:**

| Element | Value | Purpose |
|---------|-------|---------|
| Version | 2012-10-17 | Current IAM policy language version |
| Sid | AllowCloudFrontRead | Statement identifier |
| Effect | Allow | Grants permission |
| Principal | cloudfront:user/OAI | Only this OAI can access |
| Action | s3:GetObject | Read-only access |
| Resource | bucket/* | All objects in bucket |

---

## Data Flow and Request Path

### Complete Request-Response Flow

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                     COMPLETE REQUEST FLOW                                    │
└─────────────────────────────────────────────────────────────────────────────┘

CLIENT                                      AWS SERVICES
────────────────────────────────────────────────────────────────────────────

1. BROWSER
   ┌──────────────────────────────────────────────┐
   │ Request: GET /test-to-speech.png HTTP/1.1   │
   │ Host: test.talkient.app                       │
   │ Accept: image/png                             │
   │ (No cookies, no custom headers)               │
   └──────────────────────────────────────────────┘
   │
   ▼
2. DNS (Route53)
   ┌──────────────────────────────────────────────┐
   │ test.talkient.app → d2lg1os7p6l8sn.cloudfront.net
   │ (Cached locally or queried)                   │
   └──────────────────────────────────────────────┘
   │
   ▼
3. CLOUDFRONT EDGE LOCATION (Nearest)
   ┌──────────────────────────────────────────────┐
   │ Request received at edge location            │
   │                                              │
   │ Cache Key: test-to-speech.png               │
   │                                              │
   │ Check: Is object in edge cache?             │
   │   YES → Return cached object (Go to step 8) │
   │   NO  → Continue to origin request           │
   └──────────────────────────────────────────────┘
   │
   ▼ (Cache Miss)
4. CLOUDFRONT ORIGIN REQUEST
   ┌──────────────────────────────────────────────┐
   │ Build request to S3 origin:                  │
   │                                              │
   │ GET /test-to-speech.png HTTP/1.1            │
   │ Host: henriqueholtz-cloudfront-test.s3...   │
   │ X-Amz-Cf-Id: cf_id_abc123...                 │
   │ Origin-Access-Identity: cloudfront://        │
   │         E1MVPTRTOI863N                       │
   └──────────────────────────────────────────────┘
   │
   ▼
5. S3 BUCKET (Private)
   ┌──────────────────────────────────────────────┐
   │ Validate OAI access:                         │
   │                                              │
   │ 1. Extract OAI ID from request               │
   │ 2. Check bucket policy for Allow statement  │
   │ 3. Verify Principal matches OAI ARN         │
   │ 4. Check Action = s3:GetObject                │
   │                                              │
   │ If authorized: Read object from storage      │
   │ If denied: Return 403 Forbidden               │
   │                                              │
   │ Return:                                       │
   │   HTTP/1.1 200 OK                            │
   │   x-amz-server-side-encryption: AES256       │
   │   Content-Type: image/png                    │
   │   [Image Data]                                │
   └──────────────────────────────────────────────┘
   │
   ▼
6. CLOUDFRONT CACHING
   ┌──────────────────────────────────────────────┐
   │ Receive response from S3:                    │
   │                                              │
   │ • Store in edge cache                        │
   │ • Apply cache headers (TTL: 1 day default)  │
   │ • Prepare response to client                │
   └──────────────────────────────────────────────┘
   │
   ▼
7. CLOUDFRONT RESPONSE TO CLIENT
   ┌──────────────────────────────────────────────┐
   │ HTTP/1.1 200 OK                              │
   │ Content-Type: image/png                      │
   │ Content-Length: 48828                       │
   │ ETag: "abc123..."                            │
   │ Cache-Control: max-age=86400                 │
   │ Server: CloudFront                           │
   │                                              │
   │ [Image Data - 48828 bytes]                   │
   └──────────────────────────────────────────────┘
   │
   ▼
8. BROWSER
   ┌──────────────────────────────────────────────┐
   │ • Receives image                            │
   │ • Renders image on page                     │
   │ • May cache locally                         │
   └──────────────────────────────────────────────┘
```

---

## CloudFormation Template Analysis

### Resource Dependency Graph

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    RESOURCE CREATION ORDER                                   │
└─────────────────────────────────────────────────────────────────────────────┘

                         ┌─────────────────────┐
                         │   Parameters         │
                         │   (Input Values)     │
                         └──────────┬────────────┘
                                    │
         ┌───────────────────────────┼───────────────────────────┐
         │                           │                           │
         ▼                           ▼                           ▼
┌─────────────────────┐   ┌─────────────────────┐   ┌─────────────────────┐
│   CloudFrontOAI     │   │    ImageBucket      │   │   Route53Record     │
│   (Independent)     │   │    (Independent)     │   │   (Depends on:      │
│                     │   │                     │   │    CloudFront)      │
└─────────┬───────────┘   └──────────┬──────────┘   └──────────┬────────────┘
          │                          │                          │
          │                          │                          │
          │                          ▼                          │
          │               ┌─────────────────────┐              │
          │               │  ImageBucketPolicy  │              │
          │               │  (Depends on:       │              │
          │               │   ImageBucket,      │              │
          │               │   CloudFrontOAI)    │              │
          │               └──────────┬──────────┘              │
          │                          │                          │
          └──────────────────────────┼──────────────────────────┘
                                     │
                                     ▼
                          ┌─────────────────────┐
                          │  CloudFrontDist     │
                          │  (Depends on:       │
                          │   ImageBucket,      │
                          │   CloudFrontOAI,    │
                          │   ACM Certificate)  │
                          └──────────┬──────────┘
                                     │
                                     ▼
                          ┌─────────────────────┐
                          │  Route53Record      │
                          │  (Depends on:       │
                          │   CloudFrontDist)   │
                          └──────────┬──────────┘
                                     │
                                     ▼
                          ┌─────────────────────┐
                          │     Outputs         │
                          │   (Stack results)   │
                          └─────────────────────┘
```

### Parameter Validation

```yaml
Parameters:
  BucketName:
    Type: String
    Description: Globally unique S3 bucket name for storing images
    AllowedPattern: '^[a-z0-9][a-z0-9-]{1,61}[a-z0-9]$'
    # • Must be 3-63 characters
    # • Lowercase letters, numbers, hyphens
    # • Cannot have consecutive hyphens
    # • Cannot start or end with hyphen

  DomainName:
    Type: String
    Description: Custom domain name for CloudFront (e.g., images.example.com)
    AllowedPattern: '^[a-z0-9][a-z0-9.-]*[a-z0-9]$'
    # • Valid domain name pattern
    # • Supports subdomains

  HostedZoneId:
    Type: AWS::Route53::HostedZone::Id
    Description: Route53 Hosted Zone ID for the domain
    # • AWS-specific parameter type
    # • Validates against existing hosted zones

  AcmCertificateArn:
    Type: String
    Description: ACM Certificate ARN for the custom domain (must be in us-east-1)
    # • Must exist in us-east-1
    # • Must be validated (ISSUED status)
```

---

## Certification-Relevant Concepts

### AWS Well-Architected Framework Pillars

This architecture demonstrates several AWS Well-Architected Framework pillars:

#### 1. Operational Excellence

| Principle | Implementation |
|-----------|---------------|
| **Infrastructure as Code** | CloudFormation template for reproducible deployments |
| **Automated deployments** | Shell scripts for deploy/destroy/check |
| **Monitoring** | cfn-check.sh for health verification |

#### 2. Security

| Principle | Implementation |
|-----------|---------------|
| **Security at all layers** | S3 block public access + OAI + HTTPS |
| **Encrypting data at rest** | S3 SSE-AES256 |
| **Encrypting data in transit** | TLS 1.2+ required |
| **Least privilege access** | Bucket policy grants only s3:GetObject to OAI |

#### 3. Reliability

| Principle | Implementation |
|-----------|---------------|
| **Scalability** | CloudFront auto-scales globally |
| **Automated recovery** | CloudFormation rollback on failure |
| **Caching** | Edge caching reduces S3 load |

#### 4. Performance Efficiency

| Principle | Implementation |
|-----------|---------------|
| **Use caching** | CloudFront caching with 1-year max TTL |
| **Use edge locations** | Global CDN with 450+ locations |
| **Choose right protocols** | HTTP/2 for improved performance |

#### 5. Cost Optimization

| Principle | Implementation |
|-----------|---------------|
| **Pay for what you need** | PriceClass_100 (US/Canada/Europe only) |
| **Use serverless** | No EC2/containers needed |
| **Right-size resources** | S3 + CloudFront are pay-per-use |
| **Analyze spending** | AWS Cost Explorer |

#### 6. Sustainability

| Principle | Implementation |
|-----------|---------------|
| **Global reach with lower latency** | CloudFront reduces network travel |
| **Caching reduces backend requests** | S3 GET requests minimized |
| **No idle resources** | No running servers |

### AWS Global Infrastructure

| Component | Details |
|-----------|---------|
| **Regions** | 33 geographic areas worldwide |
| **Availability Zones** | 105 AZs across regions |
| **Edge Locations** | 450+ global points |
| **Local Zones** | 35+ for latency-sensitive workloads |
| **Wavelength Zones** | 10+ for 5G edge computing |

**This stack uses:**
- **Region**: us-east-1 (N. Virginia) - for CloudFormation, S3, Route53
- **Edge Locations**: Global (PriceClass_100: US, Canada, Europe)

### AWS Shared Responsibility Model

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    SHARED RESPONSIBILITY MODEL                                │
└─────────────────────────────────────────────────────────────────────────────┘

                        ┌─────────────────────────────┐
                        │     CUSTOMER RESPONSIBILITY  │
                        │     (Security IN the Cloud)  │
                        └─────────────────────────────┘
                                           │
    ┌──────────────────────────────────────┼──────────────────────────────────────┐
    │                                      │                                      │
    │  ┌─────────────────────────────┐      │      ┌─────────────────────────────┐ │
    │  │ Customer Data               │      │      │ Platform Applications       │ │
    │  │ • Encryption choice         │      │      │ • IAM policies              │ │
    │  │ • Access controls           │      │      │ • Security groups          │ │
    │  │ • Data classification       │      │      │ • Route53 policies          │ │
    │  └─────────────────────────────┘      │      └─────────────────────────────┘ │
    │                                      │                                      │
    │  ┌─────────────────────────────┐      │      ┌─────────────────────────────┐ │
    │  │ Application                │       │      │      Operating Systems      │ │
    │  │ • Secure design            │       │      │      (Not applicable -      │ │
    │  │ • Configuration            │       │      │       serverless arch)      │ │
    │  │ • Vulnerability mgmt       │       │      └─────────────────────────────┘ │
    │  └─────────────────────────────┘      │                                      │
    │                                      │      ┌─────────────────────────────┐ │
    │  ┌─────────────────────────────┐      │      │      Virtualization        │ │
    │  │ Network Infrastructure       │      │      │      (AWS managed)         │ │
    │  │ • VPC configuration         │      │      └─────────────────────────────┘ │
    │  │ • Security groups          │      │                                      │
    │  │ • Route tables             │      │      ┌─────────────────────────────┐ │
    │  └─────────────────────────────┘      │      │      Physical Infrastructure │ │
    │                                      │      │      (AWS managed)         │ │
    └──────────────────────────────────────┼──────────────────────────────────────┘
                                           │
                        ┌──────────────────┴──────────────────┐
                        │     AWS RESPONSIBILITY              │
                        │     (Security OF the Cloud)         │
                        └─────────────────────────────────────┘

    AWS Manages:
    ─────────────────────────────────────────────────────────────────────────────
    ✓ Physical security of data centers
    ✓ Network infrastructure
    ✓ Hypervisor/Firmware
    ✓ Storage erasure (when objects deleted)
    ✓ Hardware lifecycle
    ✓ Regional availability
```

### AWS Service Categories Relevant to This Architecture

| Category | Services Used | Alternative Services |
|----------|---------------|---------------------|
| **Storage** | S3 | EBS, EFS, FSx, Storage Gateway |
| **CDN** | CloudFront | Akamai, Fastly, Cloudflare |
| **DNS** | Route53 | CloudFlare, GoDaddy, Google DNS |
| **Certificate Management** | ACM | Let's Encrypt, third-party CA |
| **Infrastructure as Code** | CloudFormation | Terraform, Pulumi, CDK |
| **Identity** | IAM (for OAI) | Cognito, STS |

---

## Current Deployment Status

### Stack Status

| Component | Status | Notes |
|-----------|--------|-------|
| CloudFormation Stack | **CREATE_COMPLETE** | Successfully deployed |
| S3 Bucket | **Exists** | henriqueholtz-cloudfront-test |
| S3 Objects | **1 object** | text-to-speech.png (48828 bytes) |
| CloudFront Distribution | **Deployed** | ID: E1MVPTRTOI863N |

### Output Values

| Output | Value |
|--------|-------|
| BucketName | henriqueholtz-cloudfront-test |
| CloudFrontURL | https://d2lg1os7p6l8sn.cloudfront.net |
| CustomDomainURL | https://test.talkient.app |
| DistributionId | E1MVPTRTOI863N |

### Deployment Status: Operational

All deployment issues have been resolved:
- CloudFront can access S3 via OAI
- DNS resolution working via CloudFlare → Route53 delegation

---

## Troubleshooting Common Issues

### Issue Categories

| Category | Symptoms | Likely Causes |
|----------|----------|----------------|
| **403 Forbidden** | Can't access via CloudFront URL | OAI, bucket policy |
| **404 Not Found** | File doesn't exist | Wrong object key |
| **502 Bad Gateway** | Origin error | S3 bucket name/wrong region |
| **DNS Not Resolving** | Custom domain not working | NS delegation, propagation |
| **Certificate Error** | SSL warning | ACM not validated |

### Diagnostic Commands

```bash
# Check stack status
aws cloudformation describe-stacks \
  --stack-name your-stack-name \
  --query 'Stacks[0].StackStatus'

# Verify S3 bucket policy
aws s3 get-bucket-policy --bucket BUCKET_NAME --output json

# Check CloudFront distribution
aws cloudfront get-distribution --id DISTRIBUTION_ID

# Verify Route53 record
aws route53 list-resource-record-sets \
  --hosted-zone-id HOSTED_ZONE_ID \
  --query "ResourceRecordSets[?Type=='A']"

# Test image download
curl -I https://d2lg1os7p6l8sn.cloudfront.net/test-to-speech.png

# Create cache invalidation
aws cloudfront create-invalidation \
  --distribution-id E1MVPTRTOI863N \
  --paths "/*"
```

---

## Cost Considerations

### Monthly Cost Breakdown

#### Free Tier (First 12 Months - New AWS Accounts)

| Service | Free Tier | This Stack Usage | Cost |
|---------|-----------|-------------------|------|
| S3 Storage | 5 GB | ~1 GB | **$0.00** |
| S3 PUT | 2,000/month | ~100 | **$0.00** |
| S3 GET | 20,000/month | ~10,000 | **$0.00** |
| CloudFront Transfer | 1 TB | ~10 GB | **$0.00** |
| CloudFront Requests | 10M/month | ~100,000 | **$0.00** |
| Route53 | 1 hosted zone | 1 zone | **$0.50** |
| ACM | Free | 1 certificate | **$0.00** |
| **Total** | | | **~$0.50/month** |

#### After Free Tier (Estimated)

| Service | Price | Usage | Cost |
|---------|-------|-------|------|
| S3 Storage | $0.023/GB | 1 GB | $0.02 |
| S3 GET Requests | $0.0004/1,000 | 10,000 | $0.004 |
| CloudFront Transfer | $0.085/GB | 10 GB | $0.85 |
| CloudFront Requests | $0.0075/10,000 | 100,000 | $0.075 |
| Route53 | $0.50/zone | 1 zone | $0.50 |
| ACM | Free | 1 certificate | $0.00 |
| **Total** | | | **~$1.45/month** |

### Cost Optimization Strategies

| Strategy | Implementation | Savings |
|----------|----------------|---------|
| **Use CloudFront** | Caching at edge | 50-70% on S3 GET |
| **Set appropriate TTL** | 1 year max age | Reduces requests |
| **Use PriceClass_100** | US/Europe only | 30% vs. global |
| **Enable compression** | CloudFront compress: true | 30-50% bandwidth |
| **Use S3 Intelligent-Tiering** | Auto-optimize | For unpredictable access |
| **Implement versioning** | Object versioning | Easier cache invalidation |

---

## Best Practices

### Security Best Practices

1. **Always use HTTPS**
   ```yaml
   ViewerProtocolPolicy: redirect-to-https
   ```

2. **Restrict S3 access with OAI**
   - Never make S3 bucket public
   - Always use Origin Access Identity

3. **Enable S3 encryption at rest**
   ```yaml
   BucketEncryption:
     ServerSideEncryptionConfiguration:
       - ServerSideEncryptionByDefault:
           SSEAlgorithm: AES256
   ```

4. **Use modern TLS versions**
   ```yaml
   MinimumProtocolVersion: TLSv1.2_2021
   ```

5. **Enable S3 Block Public Access**
   ```yaml
   PublicAccessBlockConfiguration:
     BlockPublicAcls: true
     BlockPublicPolicy: true
     IgnorePublicAcls: true
     RestrictPublicBuckets: true
   ```

### Performance Best Practices

1. **Use appropriate cache TTL**
   ```yaml
   # Use managed cache policy (CachingOptimized)
   CachePolicyId: 658327ea-f89d-4fab-a63d-7e88639e58f6
   ```

2. **Enable compression**
   ```yaml
   Compress: true
   ```

3. **Use HTTP/2**
   ```yaml
   HttpVersion: http2
   ```

4. **Choose right price class**
   ```yaml
   PriceClass: PriceClass_100  # For US/Canada/Europe
   ```

### Operational Best Practices

1. **Use infrastructure as code**
   - CloudFormation for reproducible deployments
   - Version control for templates

2. **Monitor with CloudWatch**
   - Set up billing alerts
   - Monitor CloudFront requests and errors

3. **Plan for disaster recovery**
   - S3 versioning for object recovery
   - Cross-region replication for critical data (optional)

4. **Implement proper naming conventions**
   - Use descriptive resource names
   - Include environment in bucket names

---

## Additional Resources

### AWS Documentation

- [S3 Documentation](https://docs.aws.amazon.com/s3/)
- [CloudFront Documentation](https://docs.aws.amazon.com/cloudfront/)
- [Route 53 Documentation](https://docs.aws.amazon.com/route53/)
- [CloudFormation Documentation](https://docs.aws.amazon.com/cloudformation/)
- [ACM Documentation](https://docs.aws.amazon.com/acm/)

### AWS Whitepapers

- [AWS Well-Architected Framework](https://docs.aws.amazon.com/wellarchitected/)
- [AWS Security Best Practices](https://docs.aws.amazon.com/whitepapers/latest/aws-security-best-practices/)
- [Amazon CloudFront Storage Gateway](https://docs.aws.amazon.com/whitepapers/)

### Certification Resources

- [AWS Solutions Architect Associate Exam Guide](https://aws.amazon.com/certification/certified-solutions-architect-associate/)
- [AWS Skill Builder](https://skillbuilder.aws/)
- [AWS Cloud Practitioner Essentials](https://aws.amazon.com/training/path-cloud-practitioner/)

---

## Appendix: CloudFormation Template Reference

### Complete Resource Map

```
CloudFormation Template: stack.yaml
────────────────────────────────────────────────────────────────────────────

Parameters (4)
├── BucketName        → S3 bucket name
├── DomainName        → Custom domain for CloudFront
├── HostedZoneId      → Route53 hosted zone
└── AcmCertificateArn → SSL certificate (must be us-east-1)

Resources (5)
├── ImageBucket (AWS::S3::Bucket)
│   ├── Properties
│   │   ├── BucketName
│   │   ├── PublicAccessBlockConfiguration
│   │   └── BucketEncryption
│   └── DependsOn: (none - parallel creation)
│
├── ImageBucketPolicy (AWS::S3::BucketPolicy)
│   ├── Properties
│   │   ├── Bucket: !Ref ImageBucket
│   │   └── PolicyDocument (allows OAI GetObject)
│   └── DependsOn: ImageBucket, CloudFrontOAI
│
├── CloudFrontOAI (AWS::CloudFront::CloudFrontOriginAccessIdentity)
│   ├── Properties
│   │   └── CloudFrontOriginAccessIdentityConfig
│   │       └── Comment: "OAI for ${BucketName}"
│   └── DependsOn: (none)
│
├── CloudFrontDistribution (AWS::CloudFront::Distribution)
│   ├── Properties
│   │   ├── DistributionConfig
│   │   │   ├── Enabled: true
│   │   │   ├── Aliases
│   │   │   ├── PriceClass: PriceClass_100
│   │   │   ├── ViewerCertificate (ACM)
│   │   │   ├── DefaultCacheBehavior
│   │   │   └── Origins (S3 with OAI)
│   └── DependsOn: ImageBucket, CloudFrontOAI
│
└── Route53Record (AWS::Route53::RecordSet)
    ├── Properties
    │   ├── HostedZoneId
    │   ├── Name (DomainName)
    │   ├── Type: A
    │   └── AliasTarget → CloudFront
    └── DependsOn: CloudFrontDistribution

Outputs (4)
├── BucketName        → !Ref ImageBucket
├── CloudFrontURL     → https://${CloudFrontDistribution.DomainName}
├── CustomDomainURL   → https://${DomainName}
└── DistributionId   → !Ref CloudFrontDistribution
```

---

*Document Version: 1.0*  
*Last Updated: February 2026*  
*Purpose: AWS Solutions Architect Associate Certification Preparation*
