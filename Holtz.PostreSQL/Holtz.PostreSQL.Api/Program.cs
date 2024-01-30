using Holtz.PostreSQL.Api.Context;
using Holtz.PostreSQL.Api.Interfaces;
using Holtz.PostreSQL.Api.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// PostgreSQL
var Configuration = builder.Configuration;
builder.Services.AddDbContext<HoltzPostgreSqlContext>(options =>
        options.UseNpgsql(Configuration.GetConnectionString("Holtz.PostreSQL.Api")));

builder.Services.AddTransient<IPersonRepository, PersonRepository>();

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

public partial class ProgramApiMarker { }