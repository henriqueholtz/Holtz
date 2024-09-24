terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 4.0"
    }
  }
}

# Configure the AWS Provider
provider "aws" {
  region  = "us-east-1"
}

# Creating AWS SQS queue as standard
resource "aws_sqs_queue" "sh_queue" {
  name  = "customers"
}