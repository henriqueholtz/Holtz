# Create the S3 bucket
resource "aws_s3_bucket" "holtz_s3_bucket" {
  bucket = "holtz.s3" 
  
  tags = {
    Name = "Holtz.S3"
  }
}
