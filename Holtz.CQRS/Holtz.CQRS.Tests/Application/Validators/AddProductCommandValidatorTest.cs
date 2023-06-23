using FluentValidation.TestHelper;
using Holtz.CQRS.Application.Commands.AddProduct;

namespace Holtz.CQRS.Tests.Application.Validators
{
    public class AddProductCommandValidatorTest
    {
        public static IEnumerable<object[]> AddProductCommandsInvalid()
        {
            yield return new object[] { new AddProductCommand { Price = 8 } };
            yield return new object[] { new AddProductCommand { Name = "Valid name", Description = "Valid description", Price = -2 } };
            yield return new object[] { new AddProductCommand { Name = "Valid name", Description = string.Empty, Price = 3 } };
        }

        [Theory]
        [MemberData(nameof(AddProductCommandsInvalid))]
        public void ShouldBeInValid_AddProductCommand(AddProductCommand command)
        {
            // Arrange
        AddProductCommandValidator validator = new AddProductCommandValidator();

            // Act
            TestValidationResult<AddProductCommand> validatorResult = validator.TestValidate(command);

            // Assert
            Assert.False(validatorResult.IsValid);
        }
    }
}
