using Holtz.Sqs.Shared.Messages;
using MediatR;

namespace Holtz.Sqs.Application.Commands;

public record CustomerDeletedCommand : CustomerDeleted, IRequest
{

}
