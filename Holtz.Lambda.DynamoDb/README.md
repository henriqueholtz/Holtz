# Holtz.Lambda.DynamoDb

This is a simples AWS Lambda built with c# which is triggered by a change on DynamoDb.

- :heavy_check_mark: .NET 8
- :heavy_check_mark: AWS Lambda
- :heavy_check_mark: Terraform
- :heavy_check_mark: Unit tests
- :heavy_check_mark: DynamoDb table
- :heavy_check_mark: [Github Action to run tests](../.github/workflows/Holtz.Lambda.DynamoDb.yml)

## Setup

1. Create an IAM user on your AWS account with the roles `AWSLambda_FullAccess`, `AmazonDynamoDBFullAccess` and `IAMFullAccess` and generate an access key to it;
2. Install all the requirements;
3. Configure your AWS CLI with the access key using `aws configure`;
4. Pack the lambda using `cd src/Holtz.Lambda.DynamoDb/ && dotnet lambda package -o ./publish/HoltzLambdaDynamoDb.zip && cd ../../`
5. Initialize your terraform using `terraform init` (Make sure you're in the folder `{...}/Holtz/Holtz.Lambda.DynamoDb/`);
6. Apply your terraform using `terraform apply` (you need to verify carefully and type "yes" and only then the resources will be created on AWS).

## Teardown

After playing around with the application you'll want to teardown everything. To remove all AWS-related resources you just need to run `terraform destroy`, verify and confirm it.

Also you should want to remove the user and/or its access key.

##### AWS Commands

- `aws configure`
- `aws dynamodb list-tables`
