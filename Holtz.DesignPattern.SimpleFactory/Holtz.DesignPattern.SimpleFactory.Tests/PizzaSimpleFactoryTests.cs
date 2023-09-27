namespace Holtz.DesignPattern.SimpleFactory.Tests
{
    public class PizzaSimpleFactoryTests
    {
        [Fact]
        public void ShouldCreateAPepperoniPizza()
        {
            Pizza pizza = PizzaSimpleFactory.CreatePizza("Pepperoni");
            Assert.True(pizza.Name == "Pepperoni Pizza");
        }

        [Fact]
        public void ShouldCreateABaconPizza()
        {
            Pizza pizza = PizzaSimpleFactory.CreatePizza("Bacon");
            Assert.True(pizza.Name == "Bacon Pizza");
        }

        [Fact]
        public void ShouldNotCreateAPizzaInvalidType()
        {
            Assert.Throws<ApplicationException>(() => PizzaSimpleFactory.CreatePizza("InvalidType"));
        }
    }
}