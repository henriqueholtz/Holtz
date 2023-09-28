using Holtz.DesignPattern.AbstractFactory.Domain.Enums;
using Holtz.DesignPattern.AbstractFactory.Domain.Products;
using Holtz.DesignPattern.AbstractFactory.Factory.AbstractFactory;
using NuGet.Frameworks;

namespace Holtz.DesignPattern.AbstractFactoryTests
{
    public class DoughAbstractFactoryTests
    {
        private readonly DoughAbstractFactory _factoryCake = DoughAbstractFactory.CreateFactoryDough(EnumDough.Cake);
        private readonly DoughAbstractFactory _factoryPizza = DoughAbstractFactory.CreateFactoryDough(EnumDough.Pizza);

        [Fact]
        public void ShouldCreateCakeOfChocolate()
        {
            DoughBase cakeChocolate = _factoryCake.CreateDough((EnumDough)EnumCake.Chocolate);

            Assert.Equal("Cake of chocolate", cakeChocolate.Name);
            Assert.Equal(EnumDough.Cake, cakeChocolate.Dough);
            Assert.True(cakeChocolate.Ingredients.Count == 2);
            Assert.Contains("Sugar", cakeChocolate.Ingredients.ToArray());
            Assert.Contains("Chocolate", cakeChocolate.Ingredients.ToArray());
        }

        [Fact]
        public void ShouldCreateCakeOfOrange()
        {
            DoughBase cakeOrange = _factoryCake.CreateDough((EnumDough)EnumCake.Orange);

            Assert.Equal("Cake of orange", cakeOrange.Name);
            Assert.Equal(EnumDough.Cake, cakeOrange.Dough);
            Assert.True(cakeOrange.Ingredients.Count == 2);
            Assert.Contains("Sugar", cakeOrange.Ingredients.ToArray());
            Assert.Contains("Orange", cakeOrange.Ingredients.ToArray());
        }

        [Fact]
        public void ShouldCreatePizzaPepperoni()
        {
            DoughBase pizzaPepperoni = _factoryPizza.CreateDough((EnumDough)EnumPizza.Pepperoni);

            Assert.Equal("Pizza of pepperoni", pizzaPepperoni.Name);
            Assert.Equal(EnumDough.Pizza, pizzaPepperoni.Dough);
            Assert.True(pizzaPepperoni.Ingredients.Count == 2);
            Assert.Contains("Pepperoni", pizzaPepperoni.Ingredients.ToArray());
            Assert.Contains("Dough", pizzaPepperoni.Ingredients.ToArray());
        }

        [Fact]
        public void ShouldCreatePizzaBacon()
        {
            DoughBase pizzaBacon = _factoryPizza.CreateDough((EnumDough)EnumPizza.Bacon);

            Assert.Equal("Pizza of bacon", pizzaBacon.Name);
            Assert.Equal(EnumDough.Pizza, pizzaBacon.Dough);
            Assert.True(pizzaBacon.Ingredients.Count == 2);
            Assert.Contains("Bacon", pizzaBacon.Ingredients.ToArray());
            Assert.Contains("Dough", pizzaBacon.Ingredients.ToArray());
        }
    }
}