var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Holtz_FusionCache_Api>("holtz-fusioncache-api");

builder.Build().Run();
