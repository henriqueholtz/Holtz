using Holtz.DesignPattern.AbstractFactory.Domain.Enums;
using Holtz.DesignPattern.AbstractFactory.Domain.Products;

namespace Holtz.DesignPattern.AbstractFactoryTests
{
    public class CakeOrangeTests
    {

        [Fact]
        public void ShouldCreateCakeOfOrange()
        {
            DoughBase cakeOrange = new CakeOrange();

            Assert.Equal("Cake of orange", cakeOrange.Name);
            Assert.Equal(EnumDough.Cake, cakeOrange.Dough);
            Assert.True(cakeOrange.Ingredients.Count == 2);
            Assert.Contains("Sugar", cakeOrange.Ingredients.ToArray());
            Assert.Contains("Orange", cakeOrange.Ingredients.ToArray());
        }
    }
}