resource "aws_lb" "nlb" {
  name               = "holtz-codedeploy-nlb"
  internal           = false # Internet-facing
  load_balancer_type = "network"
  enable_deletion_protection = false

  subnet_mapping {
    subnet_id = aws_subnet.holtz_codedeploy_subnet[0].id
  }
  subnet_mapping {
    subnet_id = aws_subnet.holtz_codedeploy_subnet[1].id
  }
}

resource "aws_lb_listener" "nlb_listener" {
  load_balancer_arn = aws_lb.nlb.arn
  port              = 8080
  protocol          = "TCP"

  default_action {
    type             = "forward"
    target_group_arn = aws_lb_target_group.nlb_target_group.arn #Will be created below.
  }
}

resource "aws_lb_target_group" "nlb_target_group" {
  name        = "holtz-codedeploy-nlb-tg"
  port        = 8080
  protocol    = "TCP"
  vpc_id      = aws_vpc.holtz_codedeploy_vpc.id 
  target_type = "ip" # Options: ["instance" "ip" "lambda" "alb"]

  health_check {
    protocol = "TCP" #Or HTTP/HTTPS if your proxy supports it.
    port     = "traffic-port"
  }
}