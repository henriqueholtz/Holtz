# Creating an AWS SQS queue as standard (DLQ)
resource "aws_sqs_queue" "deadletter_customers_queue" {
  name  = "customers-dlq"
  message_retention_seconds = 1209600 # 14 days
}

# Creating the main AWS SQS queue as standard
resource "aws_sqs_queue" "customers_queue" {
  name  = "customers"
  redrive_policy = jsonencode({
                    "deadLetterTargetArn" = aws_sqs_queue.deadletter_customers_queue.arn,
                    "maxReceiveCount" = 3
                  })
}

# Creating a policy to sqs queue to allow SNS publish a message into it
resource "aws_sqs_queue_policy" "customers_queue_policy" {
  queue_url = aws_sqs_queue.customers_queue.id
  policy = jsonencode(
        {
            "Version": "2012-10-17",
            "Id": "sqspolicy",
            "Statement": [
            {
                "Effect": "Allow",
                "Principal": {
                    "Service": "sns.amazonaws.com"
                },
                "Action": "sqs:SendMessage",
                "Resource": aws_sqs_queue.customers_queue.arn,
                "Condition": {
                    "ArnEquals": {
                        "aws:SourceArn": aws_sns_topic.customers_topic.arn
                    }
                }
            }
        ]
    })
}

# Creating a second AWS SQS queue as standard
resource "aws_sqs_queue" "customers_deleted_queue" {
  name  = "customers_deleted"
  redrive_policy = jsonencode({
                    "deadLetterTargetArn" = aws_sqs_queue.deadletter_customers_queue.arn,
                    "maxReceiveCount" = 3
                  })
}

# Creating a policy to sqs queue to allow SNS publish a message into it
resource "aws_sqs_queue_policy" "customers_deleted_queue_policy" {
  queue_url = aws_sqs_queue.customers_deleted_queue.id
  policy = jsonencode(
        {
            "Version": "2012-10-17",
            "Id": "sqspolicy",
            "Statement": [
            {
                "Effect": "Allow",
                "Principal": {
                    "Service": "sns.amazonaws.com"
                },
                "Action": "sqs:SendMessage",
                "Resource": aws_sqs_queue.customers_deleted_queue.arn,
                "Condition": {
                    "ArnEquals": {
                        "aws:SourceArn": aws_sns_topic.customers_topic.arn
                    }
                }
            }
        ]
    })
}


# Creating the SNS topic
resource "aws_sns_topic" "customers_topic" {
  name = "customers_topic"
}

# Creating the subscription between the SNS topic and the SQS queue "customers" which will get all messages
resource "aws_sns_topic_subscription" "sns_customers_topic_subscription" {
  topic_arn = aws_sns_topic.customers_topic.arn
  protocol  = "sqs"
  endpoint  = aws_sqs_queue.customers_queue.arn
  raw_message_delivery = true
}

# Creating the subscription between the SNS topic and the SQS queue "customers_deleted" which will get only messages of deleted customers
resource "aws_sns_topic_subscription" "sns_customers_deleted_topic_subscription" {
  topic_arn = aws_sns_topic.customers_topic.arn
  protocol  = "sqs"
  endpoint  = aws_sqs_queue.customers_deleted_queue.arn
  raw_message_delivery = true

  # Filter policy based on message attributes
  filter_policy = jsonencode({
    MessageType = ["CustomerDeleted"]
  })
}