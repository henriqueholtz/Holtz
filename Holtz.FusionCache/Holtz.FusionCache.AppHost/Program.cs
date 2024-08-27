var builder = DistributedApplication.CreateBuilder(args);

// Add a Redis server to the application.
var redisCache = builder.AddRedis("redis-cache"); // OOH, ADD REDIS? NICE 

builder.AddProject<Projects.Holtz_FusionCache_Api>("holtz-fusioncache-api")
    .WithReference(redisCache);

builder.Build().Run();
