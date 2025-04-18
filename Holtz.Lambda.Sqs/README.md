# Holtz.Lambda.Sqs

This is a simple AWS Lambda function built with c#. It's triggered through an AWS SQS message.

:heavy_check_mark: .NET 8
:heavy_check_mark: AWS Lambda
:heavy_check_mark: Terraform
:heavy_check_mark: Unit tests

### Requirements

- AWS CLI
- AWS Account
- .NET 8 SDK
- Terraform
- [Amazon.Lambda.Tools](https://docs.aws.amazon.com/lambda/latest/dg/csharp-package-cli.html) (`dotnet tool install -g Amazon.Lambda.Tools`)

## Setup

1. Create an IAM user on your AWS account with the roles `AWSLambda_FullAccess`, `AmazonSQSFullAccess` and `IAMFullAccess` and generate an access key to it;
2. Install all the requirements;
3. Configure your AWS CLI with the access key using `aws configure`;
4. Pack the lambda using `cd src/Holtz.Lambda.Sqs/ && dotnet lambda package -o ./publish/HoltzLambdaSqs.zip`
5. Initialize your terraform using `terraform init` (Make sure you're in the folder `{...}/Holtz/Holtz.Lambda.Sqs/`);
6. Apply your terraform using `terraform apply` (you need to verify carefully and type "yes" and only then the resources will be created on AWS).

### Teardown

After playing around with the application you'll want to teardown everything. To remove all AWS-related resources you just need to run `terraform destroy`, verify and apply it.

Also you should want to remove the user and/or its access key.
