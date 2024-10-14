# Step 1: Create IAM Role for Lambda to assume
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

# Step 2: Attach policy to the role for logging
resource "aws_iam_role_policy_attachment" "lambda_basic_execution" {
  role       = aws_iam_role.lambda_role.name
  policy_arn = "arn:aws:iam::aws:policy/service-role/AWSLambdaBasicExecutionRole"
}

# Step 3: Deploy the Lambda function
resource "aws_lambda_function" "my_lambda" {
  function_name = "HoltzLambdaS3"
  role          = aws_iam_role.lambda_role.arn
  handler       = "Holtz.Lambda.Sqs::Holtz.Lambda.Sqs.Function::FunctionHandler"  # Set to your handler
  runtime       = "dotnet8"  # Ensure you're using the correct .NET runtime version

  # Specify the Lambda deployment package (ZIP file)
  filename      = "src/Holtz.Lambda.Sqs/publish/HoltzLambdaSqs.zip"
  source_code_hash = filebase64sha256("src/Holtz.Lambda.Sqs/publish/HoltzLambdaSqs.zip")

  memory_size   = 512
  timeout       = 5  # Timeout in seconds
}
