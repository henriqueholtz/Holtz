using Microsoft.Extensions.Caching.StackExchangeRedis;
using StackExchange.Redis;
using ZiggyCreatures.Caching.Fusion;
using ZiggyCreatures.Caching.Fusion.Serialization.NewtonsoftJson;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region FusionCache + OpenTelemetry

string channelPrefix = "holtz-";

// Add Redis
builder.Services.AddStackExchangeRedisCache((redisCacheOptions) =>
{
    redisCacheOptions.ConfigurationOptions = new ConfigurationOptions
    {
        EndPoints = { { "127.0.0.1", 6379 } },
        SslHost = "127.0.0.1",
        AbortOnConnectFail = false,
        ChannelPrefix = RedisChannel.Pattern(channelPrefix)
    };
    redisCacheOptions.InstanceName = channelPrefix;
    }
);

// Add FusionCache
builder.Services.AddFusionCache()
    .WithSerializer(new FusionCacheNewtonsoftJsonSerializer())
    .WithRegisteredDistributedCache(); // Injected by ".AddStackExchangeRedisCache(...)"

// Add OpenTelemetry
builder.Services.AddOpenTelemetry()
    // SETUP TRACES
    .WithTracing(tracing => tracing
        .AddFusionCacheInstrumentation()
        .AddConsoleExporter()
    )
    // SETUP METRICS
    .WithMetrics(metrics =>
    {
        metrics.AddFusionCacheInstrumentation();
        metrics.AddConsoleExporter();
    });
#endregion

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
