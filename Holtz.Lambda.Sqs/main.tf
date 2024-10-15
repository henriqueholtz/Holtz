# Creating the AWS SQS queue as standard
resource "aws_sqs_queue" "sqs_queue" {
  name  = "holtz-lambda-sqs"
  sqs_managed_sse_enabled = false  # Disable SSE (Server Side Encryption)
}

# Create IAM Role for Lambda to assume
resource "aws_iam_role" "lambda_role" {
  name = "holtz_lambda_sqs_role"

  assume_role_policy = jsonencode({
    Version = "2012-10-17",
    Statement = [{
      Action = "sts:AssumeRole",
      Effect = "Allow",
      Principal = {
        Service = "lambda.amazonaws.com"
      },
    }]
  })
}

# Attach policy to the role for CloudWatch logging
resource "aws_iam_role_policy_attachment" "lambda_basic_execution" {
  role       = aws_iam_role.lambda_role.name
  policy_arn = "arn:aws:iam::aws:policy/service-role/AWSLambdaBasicExecutionRole"
}

# Create permission for Lambda to read from SQS queue (Lambda Execution Role)
resource "aws_iam_role_policy" "lambda_sqs_policy" {
  name   = "lambda_sqs_policy"
  role   = aws_iam_role.lambda_role.id

  policy = jsonencode({
    Version = "2012-10-17",
    Statement = [
      {
        Action = [
          "sqs:ReceiveMessage",
          "sqs:DeleteMessage",
          "sqs:GetQueueAttributes"
        ],
        Effect   = "Allow",
        Resource = aws_sqs_queue.sqs_queue.arn
      }
    ]
  })
}

# Deploy the Lambda function
resource "aws_lambda_function" "holtz_lambda_sqs" {
  function_name = "HoltzLambdaSqs"
  role          = aws_iam_role.lambda_role.arn
  handler       = "Holtz.Lambda.Sqs::Holtz.Lambda.Sqs.Function::FunctionHandler"  # Set to your handler
  runtime       = "dotnet8"  # Ensure you're using the correct .NET runtime version

  # Specify the Lambda deployment package (ZIP file)
  filename      = "src/Holtz.Lambda.Sqs/publish/HoltzLambdaSqs.zip"
  source_code_hash = filebase64sha256("src/Holtz.Lambda.Sqs/publish/HoltzLambdaSqs.zip")

  memory_size   = 512
  timeout       = 5  # Timeout in seconds
}

# Create Event Source Mapping (Trigger SQS -> Lambda)
resource "aws_lambda_event_source_mapping" "sqs_trigger" {
  event_source_arn = aws_sqs_queue.sqs_queue.arn
  function_name    = aws_lambda_function.holtz_lambda_sqs.arn
  batch_size       = 10  # Number of messages to process at once (adjustable)
  enabled          = true
}