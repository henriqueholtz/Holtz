using Holtz.DesignPattern.AbstractFactory.Domain.Enums;
using Holtz.DesignPattern.AbstractFactory.Domain.Products;

namespace Holtz.DesignPattern.AbstractFactoryTests
{
    public class CakeChocoloateTests
    {

        [Fact]
        public void ShouldCreateCakeOfChocolate()
        {
            DoughBase cakeChocolate = new CakeChocolate();

            Assert.Equal("Cake of chocolate", cakeChocolate.Name);
            Assert.Equal(EnumDough.Cake, cakeChocolate.Dough);
            Assert.True(cakeChocolate.Ingredients.Count == 2);
            Assert.Contains("Sugar", cakeChocolate.Ingredients.ToArray());
            Assert.Contains("Chocolate", cakeChocolate.Ingredients.ToArray());
        }
    }
}