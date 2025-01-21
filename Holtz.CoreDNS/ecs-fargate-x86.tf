provider "aws" {
  region = "us-east-1"
}

resource "aws_iam_role" "ecs_task_execution_role" {
  name = "ecs-task-execution-role"

  assume_role_policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Action = "sts:AssumeRole"
        Effect = "Allow"
        Principal = {
          Service = "ecs-tasks.amazonaws.com"
        }
      }
    ]
  })
}

resource "aws_iam_role_policy_attachment" "ecs_task_execution_policy" {
  role       = aws_iam_role.ecs_task_execution_role.name
  policy_arn = "arn:aws:iam::aws:policy/service-role/AmazonECSTaskExecutionRolePolicy"
}

resource "aws_cloudwatch_log_group" "ecs_log_group" {
  name              = "/ecs/coredns"
  retention_in_days = 3
}

resource "aws_ecs_cluster" "coredns_cluster" {
  name = "holtz-coredns"
}

resource "aws_ecs_task_definition" "coredns_task" {
  family                   = "holtz-coredns-x86-fargate"
  network_mode             = "awsvpc"
  requires_compatibilities = ["FARGATE"]
  cpu                      = "256"
  memory                   = "512"
  execution_role_arn       = aws_iam_role.ecs_task_execution_role.arn
  runtime_platform {
    operating_system_family = "LINUX"
    cpu_architecture        = "X86_64"
  }

  container_definitions = jsonencode([
    {
      name      = "coredns"
      image     = "henriqueholtz/holtz-core-dns:latest"
      portMappings = [
        {
          containerPort = 53
          hostPort      = 53
          protocol      = "udp"
        },
        {
          containerPort = 53
          hostPort      = 53
          protocol      = "tcp"
        }
      ]
      essential = true
      logConfiguration = {
        logDriver = "awslogs"
        options = {
          "awslogs-group"         = aws_cloudwatch_log_group.ecs_log_group.name
          "awslogs-region"        = "us-east-1"
          "awslogs-stream-prefix" = "coredns"
        }
      }
    }
  ])
}

resource "aws_ecs_service" "coredns_service" {
  name            = "coredns-x86-fargate"
  cluster         = aws_ecs_cluster.coredns_cluster.id
  task_definition = aws_ecs_task_definition.coredns_task.arn
  launch_type     = "FARGATE"

  network_configuration {
    subnets         = aws_subnet.coredns_subnet[*].id
    security_groups = [aws_security_group.coredns_sg.id]
    assign_public_ip = true
  }
  desired_count = 1
}

resource "aws_security_group" "coredns_sg" {
  name_prefix = "holtz-coredns-sg"
  vpc_id      = aws_vpc.coredns_vpc.id

  ingress {
    from_port   = 53
    to_port     = 53
    protocol    = "udp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  ingress {
    from_port   = 53
    to_port     = 53
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

resource "aws_vpc" "coredns_vpc" {
  cidr_block = "10.0.0.0/16"
}

resource "aws_internet_gateway" "coredns_igw" {
  vpc_id = aws_vpc.coredns_vpc.id
}

resource "aws_route_table" "coredns_route_table" {
  vpc_id = aws_vpc.coredns_vpc.id

  route {
    cidr_block = "0.0.0.0/0"
    gateway_id = aws_internet_gateway.coredns_igw.id
  }
}

resource "aws_route_table_association" "coredns_rta" {
  count          = 2
  subnet_id      = aws_subnet.coredns_subnet[count.index].id
  route_table_id = aws_route_table.coredns_route_table.id
}

resource "aws_subnet" "coredns_subnet" {
  count             = 2
  vpc_id            = aws_vpc.coredns_vpc.id
  cidr_block        = cidrsubnet(aws_vpc.coredns_vpc.cidr_block, 8, count.index)
  availability_zone = data.aws_availability_zones.available.names[count.index]
}

data "aws_availability_zones" "available" {}

output "cluster_name" {
  value = aws_ecs_cluster.coredns_cluster.name
}

output "service_name" {
  value = aws_ecs_service.coredns_service.name
}
