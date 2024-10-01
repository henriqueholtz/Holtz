
using MediatR;
using Microsoft.Extensions.Logging;

namespace Holtz.Sns.Application.Commands;

public class CustomerUpdatedCommandHandler : IRequestHandler<CustomerUpdatedCommand>
{
    private readonly ILogger<CustomerUpdatedCommandHandler> _logger;

    public CustomerUpdatedCommandHandler(ILogger<CustomerUpdatedCommandHandler> logger)
    {
        _logger = logger;
    }
    public Task Handle(CustomerUpdatedCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Event {nameof(CustomerUpdatedCommand)} handled. FullName: {request.FullName}");
        return Unit.Task;
    }
}
