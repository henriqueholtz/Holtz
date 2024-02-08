var builder = DistributedApplication.CreateBuilder(args);

var apiservice = builder.AddProject<Projects.Holtz_AspireStarter_ApiService>("apiservice");

builder.AddProject<Projects.Holtz_AspireStarter_Web>("webfrontend")
    .WithReference(apiservice);

builder.Build().Run();
