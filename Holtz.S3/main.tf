# Create the S3 bucket
resource "aws_s3_bucket" "holtz_s3_bucket" {
  bucket = "holtz.s3" 
  force_destroy = true
  
  tags = {
    Name = "Holtz.S3"
  }
}
