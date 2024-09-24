// See https://aka.ms/new-console-template for more information
using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using Holtz.Sqs.Publisher.Messages;

Console.WriteLine("Starting Holtz.Sqs.Publisher...");

var sqsClient = new AmazonSQSClient();

var customer = new CustomerCreated
{
    Id = Guid.NewGuid(),
    Name = "Henrique Holtz",
    Email = "henrique_holtz@hotmail.com",
    BirthDate = new DateTime(1999, 10, 07),
    GithubUsername = "henriqueholtz"
};

var queueUrlResponse = await sqsClient.GetQueueUrlAsync("customers");

var sendMessageRequest = new SendMessageRequest
{
    QueueUrl = queueUrlResponse.QueueUrl,
    MessageBody = JsonSerializer.Serialize(customer),
    MessageAttributes = new Dictionary<string, MessageAttributeValue>
    {
        {
            "MessageType", new MessageAttributeValue
            {
                DataType = "String",
                StringValue = nameof(CustomerCreated)
            }
        }
    }
};

var response = await sqsClient.SendMessageAsync(sendMessageRequest);

Console.WriteLine("Message sent!");
Console.ReadLine();