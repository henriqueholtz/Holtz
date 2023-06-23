using FluentValidation;

namespace Holtz.CQRS.Application.Commands.AddProduct
{
    /// <summary>
    /// <see cref="AddProductCommandHandler"/>
    /// </summary>
    public class AddProductCommand : IRequest<Guid>
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }

    }
    public class AddProductCommandValidator : AbstractValidator<AddProductCommand>
    {
        public AddProductCommandValidator()
        {
            RuleFor(p => p.Name).NotEmpty().MinimumLength(5).MaximumLength(255);
            RuleFor(p => p.Description).NotEmpty().MinimumLength(5).MaximumLength(255);
            RuleFor(p => p.Price).GreaterThanOrEqualTo(0);
        }
    }
}
