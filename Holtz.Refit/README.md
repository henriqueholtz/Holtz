# Holtz.Refit

- :heavy_check_mark: **.NET 8**
- :heavy_check_mark: **[Refit](https://github.com/reactiveui/refit)**
- :heavy_check_mark: **Docker support**
- :heavy_check_mark: **External API** (Using [random-data-api.com](https://random-data-api.com/documentation))
- :heavy_check_mark: **Integration testing**
- :heavy_check_mark: **WireMock**

### ECS Support (FARGATE Arm64 and x86)

It'll create the resourcers on AWS to spin up the app in both architectures (arm64/graviton and amd64/X86_64), running on AWS ECS FARGATE.

#### Requirements:

- AWS CLI
- Terraform

#### Step by step

- Create an IAM user with the following permissions, and then create its access key
  - `AmazonECS_FullAccess`
  - `AmazonVPCFullAccess`
  - `IAMFullAccess`
  - `CloudWatch` `logs:*`
- Configure it using `aws configure` (needs AWS CLI to be installed)
- Chose one option bellow and use terraform to apply and create the resources
- Test using a local container pointing to ECS FARGATE task
  - `docker run --rm -d -it --name ubuntu --dns="<IP>" ubuntu sleep infinity`
  - `docker exec -it ubuntu bash`
  - `apt update && apt install dnsutils curl -y`
  - `curl google.com`
- `terraform init`
- `terraform apply`
- `terraform destroy`
