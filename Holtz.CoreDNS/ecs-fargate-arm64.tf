
resource "aws_ecs_task_definition" "coredns_arm64_fargate_task" {
  family                   = "holtz-coredns-arm64-fargate"
  network_mode             = "awsvpc"
  requires_compatibilities = ["FARGATE"]
  cpu                      = "256"
  memory                   = "512"
  execution_role_arn       = aws_iam_role.ecs_task_execution_role.arn
  runtime_platform {
    operating_system_family = "LINUX"
    cpu_architecture        = "ARM64"
  }

  container_definitions = jsonencode([
    {
      name      = "coredns-arm64"
      image     = "henriqueholtz/holtz-core-dns:multi-arch"
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
          "awslogs-stream-prefix" = "coredns-arm"
        }
      }
    }
  ])
}

resource "aws_ecs_service" "coredns_service_arm64_fargate" {
  name            = "coredns-arm64-fargate"
  cluster         = aws_ecs_cluster.coredns_cluster.id
  task_definition = aws_ecs_task_definition.coredns_arm64_fargate_task.arn
  launch_type     = "FARGATE"

  network_configuration {
    subnets         = aws_subnet.coredns_subnet[*].id
    security_groups = [aws_security_group.coredns_sg.id]
    assign_public_ip = true
  }
  desired_count = 1
}