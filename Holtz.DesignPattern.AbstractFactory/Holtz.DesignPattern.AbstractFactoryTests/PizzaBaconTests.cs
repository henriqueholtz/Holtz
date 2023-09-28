using Holtz.DesignPattern.AbstractFactory.Domain.Enums;
using Holtz.DesignPattern.AbstractFactory.Domain.Products;

namespace Holtz.DesignPattern.AbstractFactoryTests
{
    public class PizzaBaconTests
    {

        [Fact]
        public void ShouldCreatePizzaOfBacon()
        {
            DoughBase pizzaBacon = new PizzaBacon();

            Assert.Equal("Pizza of bacon", pizzaBacon.Name);
            Assert.Equal(EnumDough.Pizza, pizzaBacon.Dough);
            Assert.True(pizzaBacon.Ingredients.Count == 2);
            Assert.Contains("Dough", pizzaBacon.Ingredients.ToArray());
            Assert.Contains("Bacon", pizzaBacon.Ingredients.ToArray());
        }
    }
}