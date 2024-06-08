var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Holtz_CQRS_Api>("holtz-cqrs-api");

builder.Build().Run();
