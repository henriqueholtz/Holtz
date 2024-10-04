using System.Net;
using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Holtz.DynamoDb.Domain;
using Holtz.DynamoDb.Domain.Interfaces;

namespace Holtz.DynamoDb.Infraestructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly IAmazonDynamoDB _dynamoDB;
    private readonly string _tableName = "customers";

    public CustomerRepository(IAmazonDynamoDB dynamoDB)
    {
        _dynamoDB = dynamoDB;
    }

    public async Task<bool> CreateAsync(Customer customer, CancellationToken cancellationToken)
    {
        customer.UpdatedAt = DateTime.UtcNow;
        var customerAsJson = JsonSerializer.Serialize(customer);
        var customerAsAttributes = Document.FromJson(customerAsJson).ToAttributeMap();

        var createItemRequest = new PutItemRequest
        {
            TableName = _tableName,
            Item = customerAsAttributes
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

    public Task<IEnumerable<Customer>> GetAllAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
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
        return JsonSerializer.Deserialize<Customer>(itemAsDocument);
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
