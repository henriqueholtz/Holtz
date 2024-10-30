var builder = DistributedApplication.CreateBuilder(args);

string rabbitMqName = "holtz-rabbitmq"; // Needs to be the same in ApiService
var rabbitMQ = builder.AddRabbitMQ(rabbitMqName);

var apiservice = builder.AddProject<Projects.Holtz_AspireStarter_ApiService>("apiservice")
    .WithReference(rabbitMQ);

builder.AddProject<Projects.Holtz_AspireStarter_Web>("webfrontend")
    .WithReference(apiservice);


builder.Build().Run();
