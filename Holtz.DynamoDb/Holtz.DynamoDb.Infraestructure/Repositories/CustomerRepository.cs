using System.Net;
using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Holtz.DynamoDb.Domain;
using Holtz.DynamoDb.Domain.Interfaces;
using Holtz.DynamoDb.Shared;
using Microsoft.Extensions.Options;

namespace Holtz.DynamoDb.Infraestructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly IAmazonDynamoDB _dynamoDB;
    private readonly IOptions<DynamoDbConfiguration> _dynamoDbConfiguration;
    private readonly string _tableName;

    public CustomerRepository(IAmazonDynamoDB dynamoDB, IOptions<DynamoDbConfiguration> dynamoDbConfiguration)
    {
        _dynamoDB = dynamoDB;
        _dynamoDbConfiguration = dynamoDbConfiguration;
        _tableName = _dynamoDbConfiguration.Value.TableName;
    }

    public async Task<bool> CreateAsync(Customer customer, CancellationToken cancellationToken)
    {
        customer.UpdatedAt = DateTime.UtcNow;
        var customerAsJson = JsonSerializer.Serialize(customer);
        var customerAsAttributes = Document.FromJson(customerAsJson).ToAttributeMap();

        var createItemRequest = new PutItemRequest
        {
            TableName = _tableName,
            Item = customerAsAttributes,
            ConditionExpression = "attribute_not_exists(pk) and attribute_not_exists(sk)"
        };

        var response = await _dynamoDB.PutItemAsync(createItemRequest, cancellationToken);
        return response.HttpStatusCode == HttpStatusCode.OK;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var identifier = id.ToString();
        var deleteItemRequest = new DeleteItemRequest
        {
            TableName = _tableName,
            Key = new Dictionary<string, AttributeValue>
            {
                { "pk", new AttributeValue { S = identifier } },
                { "sk", new AttributeValue { S = identifier } }
            }
        };

        var response = await _dynamoDB.DeleteItemAsync(deleteItemRequest, cancellationToken);
        return response.HttpStatusCode == HttpStatusCode.OK;
    }

    public async Task<IEnumerable<Customer>> GetAllAsync(CancellationToken cancellationToken)
    {
        // Warning: Using ScanRequest you can reach a high level of costs
        var scanRequest = new ScanRequest
        {
            TableName = _tableName
        };
        var response = await _dynamoDB.ScanAsync(scanRequest);
        return response.Items
            .Select(x =>
            {
                var attributeMap = Document.FromAttributeMap(x);
                var json = attributeMap.ToJson();
                return JsonSerializer.Deserialize<Customer>(json);
            })!;
    }

    public async Task<Customer?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var identifier = id.ToString();
        var getItemRequest = new GetItemRequest
        {
            TableName = _tableName,
            Key = new Dictionary<string, AttributeValue>
            {
                { "pk", new AttributeValue { S = identifier } },
                { "sk", new AttributeValue { S = identifier } }
            }
        };

        var response = await _dynamoDB.GetItemAsync(getItemRequest, cancellationToken);
        if (response.Item.Count == 0)
            return null;

        var itemAsDocument = Document.FromAttributeMap(response.Item);
        return JsonSerializer.Deserialize<Customer>(itemAsDocument.ToJson());
    }

    public async Task<bool> UpdateAsync(Customer customer, CancellationToken cancellationToken)
    {
        customer.UpdatedAt = DateTime.UtcNow;
        var customerAsJson = JsonSerializer.Serialize(customer);
        var customerAsAttributes = Document.FromJson(customerAsJson).ToAttributeMap();

        var updateItemRequest = new PutItemRequest
        {
            TableName = _tableName,
            Item = customerAsAttributes
        };

        var response = await _dynamoDB.PutItemAsync(updateItemRequest, cancellationToken);
        return response.HttpStatusCode == HttpStatusCode.OK;
    }
}
