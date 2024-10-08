using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
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
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Configure DynamoDb
if (bool.TryParse(builder.Configuration["DynamoDb:UseLocal"]?.ToString() ?? "false", out bool useDynamoDbLocal) && useDynamoDbLocal)
{
    // Requires DynamoDb running locally. See the README for more details.
    var dynamoDbConfig = new AmazonDynamoDBConfig
    {
        ServiceURL = builder.Configuration["DynamoDb:Url"]?.ToString() ?? "http://localhost:8000"
    };
    builder.Services.AddScoped<IAmazonDynamoDB, AmazonDynamoDBClient>(sp => new AmazonDynamoDBClient(dynamoDbConfig));
}
else
{
    // Will use your credentials from your AWS CLI, and the DynamoDb on AWS Cloud.
    builder.Services.AddScoped<IAmazonDynamoDB, AmazonDynamoDBClient>();
}
#endregion

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
    #region Setting up dynamoDb table
    if (useDynamoDbLocal)
    {
        string tableName = "customers";
        await using (var scope = app.Services.CreateAsyncScope())
        {
            var _dynamoDbClient = scope.ServiceProvider.GetRequiredService<IAmazonDynamoDB>();
            bool shouldCreateTable = false;
            try
            {
                // verify if the table already exists
                await _dynamoDbClient.DescribeTableAsync(new DescribeTableRequest
                {
                    TableName = tableName
                });
            }
            catch (ResourceNotFoundException)
            {
                shouldCreateTable = true;
            }

            if (shouldCreateTable)
            {
                string pkName = "pk";
                string skName = "sk";
                // Define table structure and create it
                var request = new CreateTableRequest
                {
                    TableName = tableName,
                    AttributeDefinitions = new List<AttributeDefinition>
                    {
                        new AttributeDefinition
                        {
                            AttributeName = pkName,  // Define the primary key attribute
                            AttributeType = "S"    // 'S' stands for string type
                        },
                        new AttributeDefinition
                        {
                            AttributeName = skName,  // Define the primary key attribute
                            AttributeType = "S"      // 'S' stands for string type
                        },
                    },
                    KeySchema = new List<KeySchemaElement>
                    {
                        new KeySchemaElement
                        {
                            AttributeName = pkName,   // Partition Key
                            KeyType = "HASH"          // Define as Partition Key
                        },
                        new KeySchemaElement
                        {
                            AttributeName = skName,   // Sort Key
                            KeyType = "RANGE"         // Define as Sort Key
                        }
                    },
                    ProvisionedThroughput = new ProvisionedThroughput
                    {
                        ReadCapacityUnits = 5,    // Set read capacity
                        WriteCapacityUnits = 5    // Set write capacity
                    }
                };
                Console.WriteLine($"Creating the table {tableName} on DynamoDb locally");
                var responseCreate = await _dynamoDbClient.CreateTableAsync(request);
                Console.WriteLine($"Table creation status: {responseCreate.TableDescription.TableStatus}");
            }

        }
    }
    #endregion

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ValidationExceptionMiddleware>();
app.MapControllers();
app.UseHttpsRedirection();

await app.RunAsync();
