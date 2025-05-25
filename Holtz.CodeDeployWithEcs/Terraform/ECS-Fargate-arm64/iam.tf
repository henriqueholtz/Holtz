resource "aws_iam_role" "ecs_task_execution_role" {
  name = "ecs-codedeploy-task-execution-role"

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

resource "aws_iam_role" "vpc_lattice_role" {
  name = "holtz_codedeploy_ecsInfrastructureRole"

  assume_role_policy = jsonencode({
      Version = "2012-10-17"
      Statement = [
        {
            "Sid": "AllowAccessToECSForInfrastructureManagement", 
            "Effect": "Allow", 
            "Principal": {
                "Service": "ecs.amazonaws.com" 
            }, 
            "Action": "sts:AssumeRole" 
        }
      ]
    })
}


resource "aws_iam_role_policy" "ecs_task_execution_inline_policy" {
  name   = "ecs_task_execution_ssmmessages_policy"
  role   = aws_iam_role.ecs_task_execution_role.name

  policy = jsonencode({
    "Version": "2012-10-17",
    "Statement": [
        {
            "Sid": "VisualEditor0",
            "Effect": "Allow",
            "Action": "ssmmessages:*",
            "Resource": "*"
        }
    ]
  })
}

resource "aws_iam_role_policy_attachment" "ecs_task_execution_policy" {
  for_each = toset([
    "arn:aws:iam::aws:policy/service-role/AmazonECSTaskExecutionRolePolicy", 
  ])
  policy_arn = each.value

  role       = aws_iam_role.ecs_task_execution_role.name
}


resource "aws_iam_role_policy_attachment" "vpc_lattice_policy" {
  for_each = toset([
    "arn:aws:iam::aws:policy/service-role/AmazonECSTaskExecutionRolePolicy", 
    "arn:aws:iam::aws:policy/AmazonECSInfrastructureRolePolicyForVpcLattice",
    "arn:aws:iam::aws:policy/service-role/AmazonECSInfrastructureRolePolicyForVolumes",
    "arn:aws:iam::aws:policy/service-role/AmazonECSInfrastructureRolePolicyForServiceConnectTransportLayerSecurity"
    # "arn:aws:iam::aws:policy/AmazonEC2FullAccess", 
    # "arn:aws:iam::aws:policy/AmazonS3FullAccess"
  ])
  policy_arn = each.value

  role       = aws_iam_role.vpc_lattice_role.name
}

resource "aws_iam_role" "codedeploy_role" {
  name = "codedeploy-ecs-service-role"
  assume_role_policy = jsonencode({
    Version = "2012-10-17"
    Statement = [{
      Effect = "Allow"
      Principal = {
        Service = "codedeploy.amazonaws.com"
      }
      Action = "sts:AssumeRole"
    }]
  })
}

resource "aws_iam_role_policy_attachment" "codedeploy_role_policy" {
  role       = aws_iam_role.codedeploy_role.name
  policy_arn = "arn:aws:iam::aws:policy/AWSCodeDeployRoleForECS"
}