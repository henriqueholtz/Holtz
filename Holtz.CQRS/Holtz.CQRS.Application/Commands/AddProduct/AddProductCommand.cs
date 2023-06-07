using System.ComponentModel.DataAnnotations;

namespace Holtz.CQRS.Application.Commands.AddProduct
{
    /// <summary>
    /// <see cref="AddProductCommandHandler"/>
    /// </summary>
    public class AddProductCommand : IRequest<Guid>
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name is required!")]
        [MinLength(5)]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Description is required!")]
        [MinLength(5)]
        [MaxLength(255)]
        public string Description { get; set; } = string.Empty;

        [Range(0.01, Double.MaxValue, ErrorMessage = "Invalid value! Please a positive value.")]
        public double Price { get; set; }
    }
}
