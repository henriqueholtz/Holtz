output "ecs_cluster_name" {
  value = aws_ecs_cluster.holtz_refit_cluster.name
}

output "ecs_service_name" {
  value = aws_ecs_service.holtz_refit_service_arm64_fargate.name
}

output "vpc_id" {
  value = aws_vpc.holtz_refit_vpc.id
}

output "nlb_dns_name" {
  value = aws_lb.nlb.dns_name
}