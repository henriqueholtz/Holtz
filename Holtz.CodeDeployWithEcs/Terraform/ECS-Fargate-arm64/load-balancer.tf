resource "aws_lb" "alb" {
  name               = "holtz-codedeploy-alb"
  internal           = false # Internet-facing
  load_balancer_type = "application"
  enable_deletion_protection = false

  subnet_mapping {
    subnet_id = aws_subnet.holtz_codedeploy_subnet[0].id
  }
  subnet_mapping {
    subnet_id = aws_subnet.holtz_codedeploy_subnet[1].id
  }
}

resource "aws_lb_listener" "alb_listener" {
  load_balancer_arn = aws_lb.alb.arn
  port              = 8080
  protocol          = "HTTP"

  default_action {
    type             = "forward"
    target_group_arn = aws_lb_target_group.alb_target_group.arn #Will be created below.
  }
}

resource "aws_lb_target_group" "alb_target_group" {
  name        = "holtz-codedeploy-alb-tg"
  port        = 8080
  protocol    = "HTTP"
  vpc_id      = aws_vpc.holtz_codedeploy_vpc.id 
  target_type = "ip" # Options: ["instance" "ip" "lambda" "alb"]

  health_check {
    protocol = "HTTP"
    path     = "/"
    port     = "traffic-port"
  }
}

resource "aws_lb_target_group" "alb_target_group_green" {
  name        = "holtz-codedeploy-alb-tg-green"
  port        = 80
  protocol    = "HTTP"
  vpc_id      = aws_vpc.holtz_codedeploy_vpc.id
  target_type = "ip"

  health_check {
    protocol = "HTTP"
    path     = "/"
    port     = "traffic-port"
  }
}