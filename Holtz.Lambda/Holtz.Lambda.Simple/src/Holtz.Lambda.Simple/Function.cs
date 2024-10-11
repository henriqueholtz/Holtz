using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Holtz.Lambda.Simple;

public class Function
{
    /// <summary>
    /// A simple function that says hello to the customer
    /// </summary>
    /// <param name="customer"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public string FunctionHandler(Customer customer, ILambdaContext context)
    {
        string message = $"Hello dear {customer.Name}, from c#!";
        context.Logger.LogInformation(message);
        return message;
    }
}


public record Customer
{
    public string Name { get; set; } = default!;
}