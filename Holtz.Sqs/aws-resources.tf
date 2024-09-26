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

# Creating the main AWS SQS queue as standard
resource "aws_sqs_queue" "queue" {
  name  = "customers"
  redrive_policy = jsonencode({
                    "deadLetterTargetArn" = aws_sqs_queue.deadletter_queue.arn,
                    "maxReceiveCount" = 3
                  })
}

# Creating a second AWS SQS queue as standard (DLQ)
resource "aws_sqs_queue" "deadletter_queue" {
  name  = "customers-dlq"
  message_retention_seconds = 1209600 # 14 days
}