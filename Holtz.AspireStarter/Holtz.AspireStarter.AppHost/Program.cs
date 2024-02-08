var builder = DistributedApplication.CreateBuilder(args);

var apiservice = builder.AddProject<Projects.Holtz_AspireStarter_ApiService>("apiservice");
var rabbitMQ = builder.AddRabbitMQContainer("message_with_rabbitMQ", password: "rabbitmqpass");


builder.AddProject<Projects.Holtz_AspireStarter_Web>("webfrontend")
    .WithReference(apiservice)
    .WithReference(rabbitMQ);

builder.Build().Run();
