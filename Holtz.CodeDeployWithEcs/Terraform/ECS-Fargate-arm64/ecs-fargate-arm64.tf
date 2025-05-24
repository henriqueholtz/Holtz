
resource "aws_ecs_task_definition" "holtz_codedeploy_arm64_fargate_task" {
  family                   = "holtz-codedeploy-arm64-fargate"
  network_mode             = "awsvpc"
  requires_compatibilities = ["FARGATE"]
  cpu                      = "256"
  memory                   = "512"
  execution_role_arn       = aws_iam_role.ecs_task_execution_role.arn
  task_role_arn            = aws_iam_role.ecs_task_execution_role.arn
  
  runtime_platform {
    operating_system_family = "LINUX"
    cpu_architecture        = "ARM64"
  }

  container_definitions = jsonencode([
    {
      name      = "holtz-codedeploy64"
      image     = "henriqueholtz/holtz.refit.api:multi-arch"
      portMappings = [
        {
          containerPort = 8080
          hostPort      = 8080
          protocol      = "http"
          name          = "holtz-codedeploy-port"
        }
      ]
      essential = true
      logConfiguration = {
        logDriver = "awslogs"
        options = {
          "awslogs-group"         = aws_cloudwatch_log_group.ecs_log_group.name
          "awslogs-region"        = "us-east-1"
          "awslogs-stream-prefix" = "holtz-codedeploy"
        }
      }
    }
  ])
}

resource "aws_ecs_service" "holtz_codedeploy_service_arm64_fargate" {
  name            = "holtz-codedeploy-arm64-fargate"
  cluster         = aws_ecs_cluster.holtz_codedeploy_cluster.id
  task_definition = aws_ecs_task_definition.holtz_codedeploy_arm64_fargate_task.arn
  launch_type     = "FARGATE"
  desired_count   = 1
  enable_execute_command = true

  network_configuration {
    subnets         = aws_subnet.holtz_codedeploy_subnet[*].id
    security_groups = [aws_security_group.holtz_codedeploy_sg.id]
    assign_public_ip = true
  }

  load_balancer {
    target_group_arn = aws_lb_target_group.nlb_target_group.arn
    container_name   = "holtz-codedeploy64"
    container_port   = 8080
  }
}