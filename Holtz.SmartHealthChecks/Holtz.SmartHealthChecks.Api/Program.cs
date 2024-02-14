using Holtz.SmartHealthChecks.Api.Context;
using Holtz.SmartHealthChecks.Api.DependencyInjection;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

string? connectionString = builder.Configuration.GetConnectionString("Holtz.SmartHealthChecks.Api");
var Configuration = builder.Configuration;
builder.Services.AddDbContext<HoltzSmartHealthChecksContext>(options =>
        options.UseSqlServer(connectionString));

// Add services to the container.
builder.Services.ConfigureHealthChecks(connectionString!);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapHealthChecks("/api/health");

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
