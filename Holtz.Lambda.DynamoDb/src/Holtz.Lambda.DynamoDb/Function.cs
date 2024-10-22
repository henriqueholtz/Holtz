using System.Text.Json;
using Amazon.Lambda.Core;
using Amazon.Lambda.DynamoDBEvents;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Holtz.Lambda.DynamoDb;

public class Function
{
    public void FunctionHandler(DynamoDBEvent dynamoEvent, ILambdaContext context)
    {
        context.Logger.LogInformation($"Beginning to process {dynamoEvent.Records.Count} records...");

        foreach (var record in dynamoEvent.Records)
        {
            context.Logger.LogInformation($"Event ID: {record.EventID}");
            context.Logger.LogInformation($"Event Name: {record.EventName}");

            if (record.Dynamodb.OldImage != null)
            {
                string oldImage = JsonSerializer.Serialize(record.Dynamodb.OldImage);
                context.Logger.LogInformation($"OldImage: {oldImage}");
            }

            string newImage = JsonSerializer.Serialize(record.Dynamodb.OldImage);
            context.Logger.LogInformation($"NewImage: {newImage}");
        }

        context.Logger.LogInformation("Stream processing complete.");
    }
}