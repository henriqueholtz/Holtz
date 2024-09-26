# Amazon SQS (Simple Queue Service)

- .NET 8
- Amazon SQS
- Terraform
- Dapper
- SQLite
- FluentValidation
- Middleware
- Cancellation Tokens
- BackgroundService
- MediatR pattern using IMediatR
- DLQ (Dead Letter Queue) for errors

### Requirements

- AWS CLI
- AWS Account
- .NET 8
- Terraform

### Setup

1. Create an IAM user on your AWS account with the role "AmazonSQSFullAccess" and generate an access key to it;
2. Install all the requirements;
3. Configure your AWS CLI with the access key using `aws configure`;
4. Initialize your terraform using `terraform init`;
5. Apply your terraform using `terraform apply` (you need to verify carefully and type "yes" and only then the resources will be created on AWS)

### Teardown

After playing around with the application you'll want to teardown everything. To remove all AWS-related resources you just need to run `terraform destroy`, verify and apply it.

##### AWS Commands

- `aws configure`
- `aws sqs list-queues`

##### Terraform Commands

- `terraform init`
- `terraform validate`
- `terraform plan`
- `terraform apply`
- `terraform destroy`
