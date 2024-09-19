using System.Reflection;
using Holtz.CQRS.Application.Commands.AddProduct;
using MediatR;

namespace Holtz.CQRS.Tests.Arch;

public class ApplicationTests
{
    static readonly Assembly ApplicationLayerAssembly = typeof(AddProductCommand).Assembly;

    [Fact]
    public void HandlersShouldHaveNameEndingWithHandler()
    {

        var result = Types.InAssembly(ApplicationLayerAssembly)
                          .That()
                          .ImplementInterface(typeof(IRequestHandler<,>))
                          .Should()
                          .HaveNameEndingWith("Handler")
                          .GetResult();

        Assert.NotNull(result);
        Assert.Empty(result.FailingTypes);
        Assert.True(result.IsSuccessful);
    }
    [Fact]
    public void MediatRRequestsShouldHaveNameEndingWithQueryOrCommand()
    {

        var result = Types.InAssembly(ApplicationLayerAssembly)
                          .That()
                          .ImplementInterface(typeof(IRequest<>))
                          .Should()
                          .HaveNameEndingWith("Query")
                          .Or()
                          .HaveNameEndingWith("Command")
                          .GetResult();

        Assert.NotNull(result);
        Assert.Empty(result.FailingTypes);
        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void ApplicationShouldNotHaveDependenciesOf()
    {
        var result = Types.InAssembly(ApplicationLayerAssembly)
                .That()
                .ResideInNamespace("Holtz.CQRS.Application")
                .Should()
                .NotHaveDependencyOnAny(
                    "Holtz.CQRS.Infraestructure",
                    "Holtz.CQRS.Tests",
                    "Holtz.CQRS.Tests.Arch"
                    )
                .GetResult();

        Assert.NotNull(result);
        Assert.Empty(result.FailingTypes);
        Assert.True(result.IsSuccessful);
    }
}