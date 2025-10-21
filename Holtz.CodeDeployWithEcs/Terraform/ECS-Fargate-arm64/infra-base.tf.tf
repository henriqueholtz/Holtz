provider "aws" {
  region = "us-east-1"
}

resource "aws_cloudwatch_log_group" "ecs_log_group" {
  name              = "/ecs/holtz-codedeploy"
  retention_in_days = 3
}

resource "aws_ecs_cluster" "holtz_codedeploy_cluster" {
  name = "holtz-codedeploy"
}

resource "aws_security_group" "holtz_codedeploy_sg" {
  name_prefix = "holtz-codedeploy-sg"
  vpc_id      = aws_vpc.holtz_codedeploy_vpc.id

  ingress {
    from_port   = 8080
    to_port     = 8080
    protocol    = "udp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  ingress {
    from_port   = 8080
    to_port     = 8080
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }
}

resource "aws_vpc" "holtz_codedeploy_vpc" {
  cidr_block = "10.0.0.0/16"
  tags = {
    Name = "holtz_codedeploy_vpc"
  }
}

resource "aws_internet_gateway" "holtz_codedeploy_igw" {
  vpc_id = aws_vpc.holtz_codedeploy_vpc.id
}

resource "aws_route_table" "holtz_codedeploy_route_table" {
  vpc_id = aws_vpc.holtz_codedeploy_vpc.id

  route {
    cidr_block = "0.0.0.0/0"
    gateway_id = aws_internet_gateway.holtz_codedeploy_igw.id
  }
}

resource "aws_route_table_association" "holtz_codedeploy_rta" {
  count          = 2
  subnet_id      = aws_subnet.holtz_codedeploy_subnet[count.index].id
  route_table_id = aws_route_table.holtz_codedeploy_route_table.id
}

resource "aws_subnet" "holtz_codedeploy_subnet" {
  count             = 2
  vpc_id            = aws_vpc.holtz_codedeploy_vpc.id
  cidr_block        = cidrsubnet(aws_vpc.holtz_codedeploy_vpc.cidr_block, 8, count.index)
  availability_zone = data.aws_availability_zones.available.names[count.index]
}

data "aws_availability_zones" "available" {}
