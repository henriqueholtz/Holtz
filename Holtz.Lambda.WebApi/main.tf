# Create IAM Role for Lambda to assume
resource "aws_iam_role" "lambda_role" {
  name = "holtz_lambda_web_api_role"

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

# Deploy the Lambda function
resource "aws_lambda_function" "holtz_lambda_web_api" {
  function_name = "HoltzLambdaWebApi"
  role          = aws_iam_role.lambda_role.arn
  handler       = "Holtz.Lambda.WebApi"  # Set to your handler (the namespace)
  runtime       = "dotnet8"  # Ensure you're using the correct .NET runtime version
  
  # Specify the Lambda deployment package (ZIP file)
  filename      = "Holtz.Lambda.WebApi/publish/HoltzLambdaWebApi.zip"
  source_code_hash = filebase64sha256("Holtz.Lambda.WebApi/publish/HoltzLambdaWebApi.zip")

  memory_size   = 512
  timeout       = 5  # Timeout in seconds
}

# Create Lambda Function URL
resource "aws_lambda_function_url" "lambda_function_url" {
  function_name = aws_lambda_function.holtz_lambda_web_api.function_name
  authorization_type = "NONE"
}

output "lambda_function_url" {
  value = aws_lambda_function_url.lambda_function_url.function_url
}

