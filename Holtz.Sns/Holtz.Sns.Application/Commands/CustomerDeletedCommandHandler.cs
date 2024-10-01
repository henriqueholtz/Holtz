using MediatR;
using Microsoft.Extensions.Logging;

namespace Holtz.Sns.Application.Commands;

public class CustomerDeletedCommandHandler : IRequestHandler<CustomerDeletedCommand>
{
    private readonly ILogger<CustomerDeletedCommandHandler> _logger;

    public CustomerDeletedCommandHandler(ILogger<CustomerDeletedCommandHandler> logger)
    {
        _logger = logger;
    }
    public Task Handle(CustomerDeletedCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Event {nameof(CustomerDeletedCommand)} handled. Id: {request.Id}");
        return Unit.Task;
    }
}
