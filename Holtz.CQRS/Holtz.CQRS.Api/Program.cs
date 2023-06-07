using Holtz.CQRS.Api;
using Holtz.CQRS.Api.Middlewares;
using Holtz.CQRS.Application.Queries.GetProducts;
using Holtz.CQRS.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;
using NLog.Extensions.Logging;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Build a config object, using env vars and JSON providers.
IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

// Get values from the config given their key and their target type.
string connectionString = config?.GetRequiredSection("ConnectionStrings:ApplicationContext")?.Get<string>() ?? "";

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Add all classes to the Mediator by its Assembly
builder.Services.AddMediatR(sfg => sfg.RegisterServicesFromAssembly(typeof(GetProductsQueryHandler).GetTypeInfo().Assembly));

// Swagger
builder.Services.AddSwaggerGen();

// Add data base context
builder.Services.AddDbContext<ApplicationContext>(opt => opt.UseSqlite(config?.GetConnectionString("ApplicationContext")));

builder.Services.InjectRepositories();

// Logging
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.SetMinimumLevel(LogLevel.Debug);
    loggingBuilder.AddNLog(config);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run();
