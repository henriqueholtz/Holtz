# Summary

| Descriptions                               |  Free Tier   | Total Cost ($/month) |
| :----------------------------------------- | :----------: | :------------------: |
| CloudFormation (yml)                       |     Yes      |        $0.00         |
| AWS S3 private bucket                      | 5GB/12months |        ~$0.02        |
| Cloud Front + Origin Access Identity (OAI) | 1TB/12months |        ~$0.09        |
| Amazon Certificate Manager                 |     Yes      |        $0.00         |
| Route 53 (public host zone)                |      No      |        $0.50         |
| Custom DNS (managed via CloudFlare)        |     Yes      |        $0.00         |

## Custom DNS (via CloudFlare)

- `talkient.app` is managed via CloudFlare
- 1x CNAME to allow ACM issue/generate the certificate
- 4x NS

## Amazon Certificate Manager

- Request a public certificate for a subdomain
- No export (disabled)
- Eligible for automatic renewal

## Rote 53

- Public hosted zone for a subdomain (manual)

## CloudFormation

Resources:

- CloudFrontDistribution
- CloudFrontOAI
- ImageBucket + ImageBucketPolicy
- Route53Record

## AWS S3

- Private and simple bucket (no versioning, Object lock, CORS, etc)
- Bucket Policy (JSON) to allow CloudFront to react the objects

## Cloud Front

- Distribution with custom domain + SSL
- No WAF/DDos protection settings
- Single origin poiting to the S3 domain (which is private) using OAI
- Behaviors: Only redict HTTP to HTTPS

## Output

- `https://test.talkient.app/text-to-speech.png` -> Accessible/OK
- `https://d2lg1os7p6l8sn.cloudfront.net/text-to-speech.png` => Accessible/OK
- `https://henriqueholtz-cloudfront-test.s3.us-east-1.amazonaws.com/text-to-speech.png` => AccessDenied
