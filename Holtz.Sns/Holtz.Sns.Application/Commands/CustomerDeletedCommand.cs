using Holtz.Sns.Shared.Messages;
using MediatR;

namespace Holtz.Sns.Application.Commands;

public record CustomerDeletedCommand : CustomerDeleted, IRequest { }