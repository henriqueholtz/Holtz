// See https://aka.ms/new-console-template for more information
using Amazon.SQS;
using Amazon.SQS.Model;

// Option 1: dotnet run --project Holtz.Sns.Consumer/Holtz.Sns.Consumer.csproj
// Option 2: dotnet run --project Holtz.Sns.Consumer/Holtz.Sns.Consumer.csproj customers2
var queueName = args.Length == 1 ? args[0] : "customers";
Console.WriteLine($"Starting Holtz.Sns.Consumer... Reading from queue {queueName}...");

var sqsClient = new AmazonSQSClient();
var queueUrlResponse = await sqsClient.GetQueueUrlAsync(queueName);

var receiveMessageRequest = new ReceiveMessageRequest
{
    QueueUrl = queueUrlResponse.QueueUrl,
    AttributeNames = new List<string> { "All" },
    MessageAttributeNames = new List<string> { "All" }
};

var cancellationTokenSource = new CancellationTokenSource();
while (!cancellationTokenSource.IsCancellationRequested)
{
    var response = await sqsClient.ReceiveMessageAsync(receiveMessageRequest, cancellationTokenSource.Token);
    foreach (Message message in response.Messages)
    {
        Console.WriteLine($"Message ID: {message.MessageId}");
        Console.WriteLine($"Message Body: {message.Body}");

        await sqsClient.DeleteMessageAsync(queueUrlResponse.QueueUrl, message.ReceiptHandle);
    }

    await Task.Delay(3000);
}