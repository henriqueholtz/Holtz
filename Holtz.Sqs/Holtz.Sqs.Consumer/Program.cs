using Amazon.SQS;
using Amazon.SQS.Model;

var cancellationTokenSource = new CancellationTokenSource();
Console.WriteLine("Starting Holtz.Sqs.Consumer...");

var sqsClient = new AmazonSQSClient();
var queueUrlResponse = await sqsClient.GetQueueUrlAsync("customers");

var receiveMessageRequest = new ReceiveMessageRequest
{
    QueueUrl = queueUrlResponse.QueueUrl,
    AttributeNames = new List<string> { "All" },
    MessageAttributeNames = new List<string> { "All" }
};

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