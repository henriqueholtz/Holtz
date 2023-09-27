namespace Holtz.DesignPattern.SimpleFactory.Tests
{
    public class BaconPizzaTests
    {
        [Fact]
        public void ShouldBakePizza()
        {
            //arrange
            Pizza baconPizza = new BaconPizza();

            //act
            string result = baconPizza.Bake();

            //assert
            Assert.Equal("Baking a bacon pizza...", result);
        }

        [Fact]
        public void ShouldPreparePizza()
        {
            //arrange
            Pizza baconPizza = new BaconPizza();

            //act
            string result = baconPizza.Prepare();

            //assert
            Assert.Equal("Preparing a bacon pizza...", result);
        }

        [Fact]
        public void ShouldPackPizza()
        {
            //arrange
            Pizza baconPizza = new BaconPizza();

            //act
            string result = baconPizza.Pack();

            //assert
            Assert.Equal("Packing a bacon pizza...", result);
        }
    }
}