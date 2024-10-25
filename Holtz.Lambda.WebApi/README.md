# Holtz.Lambda.WebApi

- :heavy_check_mark: .NET 8
- :heavy_check_mark: AWS Lambda
- :heavy_check_mark: Terraform

### Requirements

- AWS CLI
- AWS Account
- .NET 8 SDK
- Terraform

## Setup

1. Create an IAM user on your AWS account with the roles `AWSLambda_FullAccess` and `IAMFullAccess` and generate an access key to it;
2. Install all the requirements;
3. Configure your AWS CLI with the access key using `aws configure`;
4. Pack the lambda using `cd Holtz.Lambda.WebApi/ && dotnet lambda package -o ./publish/HoltzLambdaWebApi.zip && cd ..`
5. Initialize your terraform using `terraform init` (Make sure you're in the folder `{...}/Holtz/Holtz.Lambda.WebApi/`);
6. Apply your terraform using `terraform apply` (you need to verify carefully and type "yes" and only then the resources will be created on AWS).
7. Copy the url from output and make an HTTP request to `{url}/weatherforecast`

### Teardown

After playing around with the application you'll want to teardown everything. To remove all AWS-related resources you just need to run `terraform destroy`, verify and apply it.

Also you should want to remove the user and/or its access key.
