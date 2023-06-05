using Holtz.CQRS.Application.Interfaces;
using Holtz.CQRS.Application.Queries.GetProducts;
using Holtz.CQRS.Infraestructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Add all classes to the Mediator by its Assembly
builder.Services.AddMediatR(sfg => sfg.RegisterServicesFromAssembly(typeof(GetProductsQueryHandler).GetTypeInfo().Assembly));

// Swagger
builder.Services.AddSwaggerGen();

// Add data base context
builder.Services.AddSingleton<IApplicationContext, ApplicationContext>();

var app = builder.Build();

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
