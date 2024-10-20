# Holtz.S3

This is a simple Web Api built with c# and .NET 8 to show an integration with Amazon S3. It also uses Dapper (ORM) + SQLite and Terraform.

- :heavy_check_mark: .NET 8
- :heavy_check_mark: AWS S3
- :heavy_check_mark: SQLite
- :heavy_check_mark: Dapper
- :heavy_check_mark: Terraform

## Requirements

- .NET 8+
- AWS account
- AWS CLI
- Terraform

### Setup using AWS + Terraform

1. Create an IAM user on your own AWS account with the role `AmazonS3FullAccess`. After creating the user, generate its access key;
2. Install all the requirements;
3. Configure your AWS CLI with the access key using `aws configure`;
4. Initialize your terraform using `terraform init`;
5. Apply your terraform using `terraform apply` (you need to verify carefully and type "yes" and only then the resources will be created on AWS)
6. Ready! Just run the `Holtz.S3` project and use it.

### Teardown

After playing around with the application you'll want to teardown everything. To remove all AWS-related resources you just need to run `terraform destroy`, verify and apply it.

Also you should want to remove the user and/or its access key.

##### AWS Commands

- `aws configure`
- `aws s3api list-buckets`

##### Terraform Commands

- `terraform init`
- `terraform validate`
- `terraform plan`
- `terraform apply`
- `terraform destroy`
