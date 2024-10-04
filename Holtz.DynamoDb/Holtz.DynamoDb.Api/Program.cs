using Amazon.DynamoDBv2;
using FluentValidation.AspNetCore;
using Holtz.DynamoDb.Api.Middlewares;
using Holtz.DynamoDb.Application;
using Holtz.DynamoDb.Application.Interfaces;
using Holtz.DynamoDb.Domain.Interfaces;
using Holtz.DynamoDb.Infraestructure.Repositories;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddFluentValidationAutoValidation(x =>
{
    x.DisableDataAnnotationsValidation = true;
}); ;
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAmazonDynamoDB, AmazonDynamoDBClient>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IGitHubService, GitHubService>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

builder.Services.AddHttpClient("GitHub", httpClient =>
{
    httpClient.BaseAddress = new Uri(builder.Configuration.GetValue<string>("GitHub:ApiBaseUrl")!);
    httpClient.DefaultRequestHeaders.Add(
        HeaderNames.Accept, "application/vnd.github.v3+json");
    httpClient.DefaultRequestHeaders.Add(
        HeaderNames.UserAgent, $"Course-{Environment.MachineName}");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ValidationExceptionMiddleware>();
app.MapControllers();
app.UseHttpsRedirection();

await app.RunAsync();
