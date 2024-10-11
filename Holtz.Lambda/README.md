# Holtz.Lambda

## Requirements

- .NET 8+ SDK
- AWS Account
- AWS CLI or .NET CLI

### Setup

1. Create an IAM user on your AWS account with the roles `AWSLambda_FullAccess`. After creating the user, generate its access key;
2. Install all the requirements;
3. Configure your AWS CLI with the access key using `aws configure`;

### Commands using AWS CLI

- `aws lambda list-functions`
- Example to run a lambda: `aws lambda invoke --function-name {functionName} --cli-binary-format raw-in-base64-out --payload '{ "Hello": "From the console" }' response.json`

### Commands using .NET CLI

- Install the tool: `dotnet tool install -g Amazon.Lambda.Tools`
- Example to run a lambda: `dotnet lambda invoke-function {functionName} --payload '{ "Hello": "From the console" }'`
- Install the lambda templates for .NET projects: `dotnet new -i Amazon.Lambda.Templates`
- Create/Deploy the Lambda at AWS `cd Holtz.Lambda.Simple/src/Holtz.Lambda.Simple/ && dotnet lambda deploy-function HoltzLambdaSimple`
- Run the Lambda: `dotnet lambda invoke-function HoltzLambdaSimple --payload '{ "Name": "Henrique" }'`
