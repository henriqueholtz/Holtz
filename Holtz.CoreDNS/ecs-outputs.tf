output "cluster_name" {
  value = aws_ecs_cluster.coredns_cluster.name
}

output "service_name" {
  value = aws_ecs_service.coredns_service_x86_fargate.name
}
