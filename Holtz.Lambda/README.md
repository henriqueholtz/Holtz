# Holtz.Lambda

## Requirements

- .NET 8+ SDK
- AWS Account
- AWS CLI or .NET CLI
- [Amazon.Lambda.Tools](https://docs.aws.amazon.com/lambda/latest/dg/csharp-package-cli.html) (`dotnet tool install -g Amazon.Lambda.Tools`)

### Setup

1. Create an IAM user on your AWS account with the roles `AWSLambda_FullAccess`. After creating the user, generate its access key;
2. Install all the requirements;
3. Configure your AWS CLI with the access key using `aws configure`;

### Commands using AWS CLI

- `aws lambda list-functions`
- Example to run a lambda: `aws lambda invoke --function-name {functionName} --cli-binary-format raw-in-base64-out --payload '{ "Hello": "From the console" }' response.json`

### Commands using .NET CLI

- Install the tool to create/deploy: `dotnet tool install -g Amazon.Lambda.Tools`
- Example to run a lambda: `dotnet lambda invoke-function {functionName} --payload '{ "Hello": "From the console" }'`
- Install the lambda templates for .NET projects: `dotnet new -i Amazon.Lambda.Templates`
- Create/Deploy the Lambda at AWS `cd Holtz.Lambda.Simple/src/Holtz.Lambda.Simple/ && dotnet lambda deploy-function HoltzLambdaSimple`
- Run the Lambda: `dotnet lambda invoke-function HoltzLambdaSimple --payload '{ "Name": "Henrique" }'`

### How to debug

1. Install the tool to debug (for .NET 8): `dotnet tool install -g Amazon.Lambda.TestTool-8.0`
2. Run the test tool lambda `cd Holtz.Lambda.Simple/src/Holtz.Lambda.Simple && dotnet lambda-test-tool-8.0` (here the lambda is also running)
3. Attach a debugger to the process (tip: search for `dotnet-lambda`)
