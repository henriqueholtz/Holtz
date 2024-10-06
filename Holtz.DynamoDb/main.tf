resource "aws_dynamodb_table" "customers_table" {
  name         = "customers"
  billing_mode = "PROVISIONED"    # Use Provisioned Mode

  read_capacity  = 1              # Read Capacity Units
  write_capacity = 1              # Write Capacity Units

  attribute {
    name = "pk"                   # Partition key
    type = "S"                    # S = String
  }

  attribute {
    name = "sk"                   # Sort key (range key)
    type = "S"                    # S = String
  }

  hash_key  = "pk"                # Partition key
  range_key = "sk"                # Sort key (optional)

  tags = {
    Name        = "Holtz.DynamoDb"
    Environment = "Prod"
  }
}