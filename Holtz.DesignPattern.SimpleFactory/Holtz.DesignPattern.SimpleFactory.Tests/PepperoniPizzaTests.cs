namespace Holtz.DesignPattern.SimpleFactory.Tests
{
    public class PepperoniPizzaTests
    {
        [Fact]
        public void ShouldBakePizza()
        {
            //arrange
            Pizza pepperoniPizza = new PepperoniPizza();
            
            //act
            string result = pepperoniPizza.Bake();

            //assert
            Assert.Equal("Baking a pepperoni pizza...", result);
        }

        [Fact]
        public void ShouldPreparePizza()
        {
            //arrange
            Pizza pepperoniPizza = new PepperoniPizza();

            //act
            string result = pepperoniPizza.Prepare();

            //assert
            Assert.Equal("Preparing a pepperoni pizza...", result);
        }

        [Fact]
        public void ShouldPackPizza()
        {
            //arrange
            Pizza pepperoniPizza = new PepperoniPizza();

            //act
            string result = pepperoniPizza.Pack();

            //assert
            Assert.Equal("Packing a pepperoni pizza...", result);
        }
    }
}