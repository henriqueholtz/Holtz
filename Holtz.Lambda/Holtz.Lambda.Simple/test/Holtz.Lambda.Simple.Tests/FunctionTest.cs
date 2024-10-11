using Xunit;
using Amazon.Lambda.TestUtilities;

namespace Holtz.Lambda.Simple.Tests;

public class FunctionTest
{
    [Fact]
    public void TestToUpperFunction()
    {
        // Arrange
        var function = new Function();
        var context = new TestLambdaContext();
        var customer = new Customer { Name = "Henrique" };

        // Act
        string result = function.FunctionHandler(customer, context);

        // Assert
        Assert.Equal($"Hello dear {customer.Name}, from c#!", result);
    }
}
