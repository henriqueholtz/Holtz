using Holtz.Refit.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Refit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Refit
string? randomDataApiUrl = builder.Configuration.GetSection("Refit:RandomDataApiUrl").Get<string>();
if (randomDataApiUrl == null)
    throw new Exception("Invalid 'RandomDataApiUrl' from configuration file");

builder.Services.AddRefitClient<IRandomDataApi>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(randomDataApiUrl));

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
