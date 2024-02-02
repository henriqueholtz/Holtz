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
int? timeoutInMilliseconds = builder.Configuration.GetSection("Refit:TimeoutInMilliseconds").Get<int>();
if (randomDataApiUrl == null)
    throw new Exception("Invalid 'RandomDataApiUrl' from configuration file");

builder.Services.AddRefitClient<IRandomDataApi>()
    .ConfigureHttpClient(c =>
    {
        c.BaseAddress = new Uri(randomDataApiUrl);
        c.Timeout = TimeSpan.FromMilliseconds(timeoutInMilliseconds ?? 1 * 1000);
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

app.MapControllers();

app.Run();

public partial class ProgramApiMarker { }