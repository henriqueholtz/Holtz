# Holtz.Sns

- .NET 8
- AWS SNS (Simple Notification Service)
  - Topics
  - Subscriptions
  - Subscription filter policy
- AWS SQS (Simple Queue Service)
  - Pub/Sub
  - DLQ (Dead Letter Queue) for errors
- Terraform
- Dapper
- SQLite
- FluentValidation
- Middleware
- Cancellation Tokens
- BackgroundService
- MediatR pattern using IMediatR

## Requirements

- .NET 8+
- AWS account
- AWS CLI
- Terraform

### Setup

1. Create an IAM user on your AWS account with the roles `AmazonSNSFullAccess` and `AmazonSQSFullAccess`. After creating the user, generate its access key;
2. Install all the requirements;
3. Configure your AWS CLI with the access key using `aws configure`;
4. Initialize your terraform using `terraform init`;
5. Apply your terraform using `terraform apply` (you need to verify carefully and type "yes" and only then the resources will be created on AWS)

### Teardown

After playing around with the application you'll want to teardown everything. To remove all AWS-related resources you just need to run `terraform destroy`, verify and apply it.

Also you should want to remove the user and/or its access key.

##### AWS Commands

- `aws configure`
- `aws sns list-topics`
- `aws sqs list-queues`

##### Terraform Commands

- `terraform init`
- `terraform validate`
- `terraform plan`
- `terraform apply`
- `terraform destroy`

### TODOs

- Add tests like unit and/or integration and/or arch tests
