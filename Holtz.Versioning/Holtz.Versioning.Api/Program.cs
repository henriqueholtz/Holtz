using Holtz.Versioning.Api;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

#region Swagger with Versioning Api 
builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = ApiVersioning.DefaultApiVersion;
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ReportApiVersions = true;
    opt.ApiVersionReader = ApiVersionReader.Combine(new HeaderApiVersionReader(ApiVersioning.ApiVersionAsString));
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
});
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen(c =>
{
    foreach(var item in c.SwaggerGeneratorOptions.SwaggerDocs)
    {
        item.Value.Contact = new OpenApiContact { Email = "henrique_holtz@hotmail.com", Name = "Henrique Holtz" };
        item.Value.Description = $"<b>Holtz.Versioning.Api {item.Key}</b>";
    }

    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    c.CustomSchemaIds(type => type.FullName);
});
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("v1/swagger.json", "Holtz.Versioning.Api - V1");
        options.SwaggerEndpoint("v2/swagger.json", "Holtz.Versioning.Api - V2");
        options.SwaggerEndpoint("v3/swagger.json", "Holtz.Versioning.Api - V3");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
