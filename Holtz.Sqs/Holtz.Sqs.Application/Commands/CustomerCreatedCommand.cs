using Holtz.Sqs.Shared.Messages;
using MediatR;

namespace Holtz.Sqs.Application.Commands;

public record CustomerCreatedCommand : CustomerCreated, IRequest
{

}
