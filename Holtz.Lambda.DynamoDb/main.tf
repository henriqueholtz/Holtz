# Create DynamoDb table
resource "aws_dynamodb_table" "holtz_lambda_dynamodb_table" {
  name         = "holtz_lambda_dynamodb"
  billing_mode = "PROVISIONED"    # Use Provisioned Mode

  read_capacity  = 1              # Read Capacity Units
  write_capacity = 1              # Write Capacity Units

  attribute {
    name = "pk"                   # Partition key
    type = "S"                    # S = String
  }

  hash_key  = "pk"                # Partition key

  stream_enabled   = true          # Enable DynamoDB Stream
  stream_view_type = "NEW_AND_OLD_IMAGES" # Stream new and old images

  tags = {
    Name        = "Holtz.Lambda.DynamoDb"
    Environment = "Prod"
  }
}


# Create IAM Role for Lambda to assume
resource "aws_iam_role" "lambda_role" {
  name = "holtz_lambda_dynamodb_role"

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

# Attach policy for DynamoDB access
resource "aws_iam_role_policy" "lambda_dynamodb_access" {
  name   = "lambda_dynamodb_access_policy"
  role   = aws_iam_role.lambda_role.id
  policy = jsonencode({
    Version = "2012-10-17",
    Statement = [
      {
        Action = [
          "dynamodb:ListStreams",
          "dynamodb:GetRecords",
          "dynamodb:GetShardIterator",
          "dynamodb:DescribeStream",
          "dynamodb:Read"
        ],
        Effect   = "Allow",
        Resource = aws_dynamodb_table.holtz_lambda_dynamodb_table.stream_arn
      }
    ]
  })
}

# Deploy the Lambda function
resource "aws_lambda_function" "holtz_lambda_dynamodb" {
  function_name = "HoltzLambdaDynamoDb"
  role          = aws_iam_role.lambda_role.arn
  handler       = "Holtz.Lambda.DynamoDb::Holtz.Lambda.DynamoDb.Function::FunctionHandler"  # Set to your handler
  runtime       = "dotnet8"  # Ensure you're using the correct .NET runtime version

  # Specify the Lambda deployment package (ZIP file)
  filename      = "src/Holtz.Lambda.DynamoDb/publish/HoltzLambdaDynamoDb.zip"
  source_code_hash = filebase64sha256("src/Holtz.Lambda.DynamoDb/publish/HoltzLambdaDynamoDb.zip")

  memory_size   = 512
  timeout       = 5  # Timeout in seconds
}

# Create Event Source Mapping to trigger Lambda from DynamoDB Stream
resource "aws_lambda_event_source_mapping" "dynamodb_trigger" {
  event_source_arn = aws_dynamodb_table.holtz_lambda_dynamodb_table.stream_arn
  function_name    = aws_lambda_function.holtz_lambda_dynamodb.arn
  starting_position = "LATEST"

  depends_on = [aws_dynamodb_table.holtz_lambda_dynamodb_table]
}