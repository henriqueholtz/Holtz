using MediatR;
using Microsoft.Extensions.Logging;

namespace Holtz.Sns.Application.Commands;

public class CustomerCreatedCommandHandler : IRequestHandler<CustomerCreatedCommand>
{
    private readonly ILogger<CustomerCreatedCommandHandler> _logger;

    public CustomerCreatedCommandHandler(ILogger<CustomerCreatedCommandHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(CustomerCreatedCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Event {nameof(CustomerCreatedCommand)} handled. FullName: {request.FullName}");
        return Unit.Task;
    }
}
