resource "aws_ecs_task_definition" "holtz_refit_x86_fargate_task" {
  family                   = "holtz-refit-x86-fargate"
  network_mode             = "awsvpc"
  requires_compatibilities = ["FARGATE"]
  cpu                      = "256"
  memory                   = "512"
  execution_role_arn       = aws_iam_role.ecs_task_execution_role.arn
  task_role_arn            = aws_iam_role.ecs_task_execution_role.arn
  
  runtime_platform {
    operating_system_family = "LINUX"
    cpu_architecture        = "X86_64"
  }
  

  container_definitions = jsonencode([
    {
      name      = "holtz-refit"
      image     = "henriqueholtz/holtz.refit.api:multi-arch"
      portMappings = [
        {
          containerPort = 8080
          hostPort      = 8080
          protocol      = "http"
          name          = "holtz-refit2-port"
        }
      ]
      essential = true
      logConfiguration = {
        logDriver = "awslogs"
        options = {
          "awslogs-group"         = aws_cloudwatch_log_group.ecs_log_group.name
          "awslogs-region"        = "us-east-1"
          "awslogs-stream-prefix" = "holtz-refit"
        }
      }
    }
  ])
}


resource "aws_ecs_service" "holtz_refit_service_x86_fargate" {
  name            = "holtz-refit-x86-fargate"
  cluster         = aws_ecs_cluster.holtz_refit_cluster.id
  task_definition = aws_ecs_task_definition.holtz_refit_x86_fargate_task.arn
  launch_type     = "FARGATE"
  desired_count   = 1
  enable_execute_command = true

  network_configuration {
    subnets         = aws_subnet.holtz_refit_subnet[*].id
    security_groups = [aws_security_group.holtz_refit_sg.id]
    assign_public_ip = true
  }

  service_connect_configuration {
    enabled   = true
    namespace = aws_service_discovery_http_namespace.holtz_refit_namespace.arn
    service {
      discovery_name = "holtz-refit2"
      port_name      = "holtz-refit2-port"
      client_alias {
        dns_name = "random-data-api2.com"
        port     = 8080
      }
    }
  }
}
