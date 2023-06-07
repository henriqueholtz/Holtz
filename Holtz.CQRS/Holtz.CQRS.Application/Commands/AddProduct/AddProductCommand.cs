using MediatR;

namespace Holtz.CQRS.Application.Commands.AddProduct
{
    /// <summary>
    /// <see cref="AddProductCommandHandler"/>
    /// </summary>
    public class AddProductCommand : IRequest<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
    }
}
