output "ecs_cluster_name" {
  value = aws_ecs_cluster.holtz_codedeploy_cluster.name
}

output "ecs_service_name" {
  value = aws_ecs_service.holtz_codedeploy_service_arm64_fargate.name
}

output "vpc_id" {
  value = aws_vpc.holtz_codedeploy_vpc.id
}

output "alb_dns_name" {
  value = aws_lb.alb.dns_name
}