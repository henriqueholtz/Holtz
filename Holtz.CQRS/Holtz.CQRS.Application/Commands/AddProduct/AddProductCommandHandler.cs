using MediatR;

namespace Holtz.CQRS.Application.Commands.AddProduct
{
    internal class AddProductCommandHandler : IRequestHandler<AddProductCommand, long>
    {
        public Task<long> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            return Task.FromResult<long>(1);
        }
    }
}
