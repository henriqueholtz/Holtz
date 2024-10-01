// See https://aka.ms/new-console-template for more information
using System.Text.Json;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Holtz.Sns.Shared.Messages;

Console.WriteLine("Starting Holtz.Sns.Publisher...");
var customer = new CustomerCreated
{
    Id = Guid.NewGuid(),
    FullName = "Henrique Holtz",
    Email = "henrique_holtz@hotmail.com",
    BirthDate = new DateTime(1999, 10, 07),
    GitHubUsername = "henriqueholtz"
};

var snsClient = new AmazonSimpleNotificationServiceClient();

var topicArnResponse = await snsClient.FindTopicAsync("customers_topic");

var publishRequest = new PublishRequest
{
    TopicArn = topicArnResponse.TopicArn,
    Message = JsonSerializer.Serialize(customer),
    MessageAttributes = new Dictionary<string, MessageAttributeValue> {
        { "MessageType", new MessageAttributeValue {
            DataType = "String",
            StringValue = nameof(CustomerCreated)
        }}
    }
};

var response = await snsClient.PublishAsync(publishRequest);

Console.WriteLine($"Message published to SNS. Status Code: {response.HttpStatusCode}...");