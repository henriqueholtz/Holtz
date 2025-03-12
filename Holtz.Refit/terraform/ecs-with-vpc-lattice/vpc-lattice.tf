# VPC Lattice Resources
resource "aws_vpclattice_service" "holtz_refit_lattice_service" {
  name = "holtz-refit-lattice-service"
  auth_type = "NONE" # Or "AWS_IAM"
}

resource "aws_vpclattice_target_group" "holtz_refit_lattice_tg" {
  name = "holtz-refit-lattice-tg"
  type = "IP" #expected type to be one of ["IP" "LAMBDA" "INSTANCE" "ALB"]
  config {
    port = 8080
    protocol = "HTTP" # Or "HTTPS"
    vpc_identifier = aws_vpc.holtz_refit_vpc.id
    health_check {
      path = "/api/beers"
      protocol = "HTTP"
      port = 8080
    }
  }
}

resource "aws_vpclattice_listener" "holtz_refit_lattice_listener" {

  name = "holtz-refit-lattice-listener"
  protocol = "HTTP"
  service_identifier = aws_vpclattice_service.holtz_refit_lattice_service.id
  default_action {
    forward {
      target_groups {
        target_group_identifier = aws_vpclattice_target_group.holtz_refit_lattice_tg.id
      }
    }
  }
}

# resource "aws_vpclattice_rule" "holtz_refit_lattice_rule" {
#   name = "holtz_refit_lattice_rule"
#   listener_identifier = aws_vpclattice_listener.holtz_refit_lattice_listener.id
#   priority = 100
#   action {
#     target_group {
#       target_group_identifier = aws_vpclattice_target_group.holtz_refit_lattice_tg.id
#     }
#   }

#   match {
#     http_match {
#       prefix = "/" # or your desired path
#     }
#   }
# }

# Add VPC Lattice Service Association. This is required to associate the ECS service to your VPC Lattice service.

resource "aws_vpclattice_service_network_service_association" "holtz_refit_lattice_netwrok_ass" {
  service_identifier = aws_vpclattice_service.holtz_refit_lattice_service.id
  service_network_identifier = aws_vpclattice_service_network.holtz_refit_lattice_service_network.id #Replace with your service network id.
#   security_group_ids = [aws_security_group.ecs_tasks.id] #Ensure to add your ECS Task SG here.
}

resource "aws_vpclattice_service_network" "holtz_refit_lattice_service_network" {
  name      = "holtz-refit-lattice-service-network"
  auth_type = "NONE" #"AWS_IAM"
}