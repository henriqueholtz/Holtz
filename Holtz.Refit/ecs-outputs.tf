output "cluster_name" {
  value = aws_ecs_cluster.holtz_refit_cluster.name
}

output "service_name" {
  value = aws_ecs_service.holtz_refit_service_x86_fargate.name
}
