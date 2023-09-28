using Holtz.DesignPattern.AbstractFactory.Domain.Enums;
using Holtz.DesignPattern.AbstractFactory.Domain.Products;

namespace Holtz.DesignPattern.AbstractFactoryTests
{
    public class PizzaPepperoniTests
    {

        [Fact]
        public void ShouldCreatePizzaOfPepperoni()
        {
            DoughBase pizzaPepperoni = new PizzaPepperoni();

            Assert.Equal("Pizza of pepperoni", pizzaPepperoni.Name);
            Assert.Equal(EnumDough.Pizza, pizzaPepperoni.Dough);
            Assert.True(pizzaPepperoni.Ingredients.Count == 2);
            Assert.Contains("Dough", pizzaPepperoni.Ingredients.ToArray());
            Assert.Contains("Pepperoni", pizzaPepperoni.Ingredients.ToArray());
        }
    }
}