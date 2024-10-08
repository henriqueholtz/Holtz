# Holtz.DynamoDb

- .NET 8
- AWS DynamoDb
- Terraform

## Requirements

- .NET 8+
- AWS account + AWS CLI + Terraform or just Docker

### Setup using AWS + Terraform

1. Create an IAM user on your own AWS account with the role `AmazonDynamoDBFullAccess`. After creating the user, generate its access key;
2. Install all the requirements;
3. Configure your AWS CLI with the access key using `aws configure`;
4. Initialize your terraform using `terraform init`;
5. Apply your terraform using `terraform apply` (you need to verify carefully and type "yes" and only then the resources will be created on AWS)
6. Make sure the setting `DynamoDb:UseLocal` is disabled at `appsettings.json`/`appsettings.Development.json`
7. Ready! Just run the `Holtz.DynamoDb.Api` project and use it.

### Setup just using Docker

1. Run the DynamoDb as a docker container ([ref](https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/DynamoDBLocal.DownloadingAndRunning.html)). Ex:

   ```
   docker run --rm -p 8000:8000 amazon/dynamodb-local
   ```

2. Make sure the setting `DynamoDb:UseLocal` is enabled at `appsettings.json`/`appsettings.Development.json`
3. Ready! Just run the `Holtz.DynamoDb.Api` project and use it.

### Teardown

After playing around with the application you'll want to teardown everything. To remove all AWS-related resources you just need to run `terraform destroy`, verify and apply it.

Also you should want to remove the user and/or its access key.

##### AWS Commands

- `aws configure`
- `aws dynamodb list-tables`

### TODOs

- Add tests like unit and/or integration and/or arch tests
