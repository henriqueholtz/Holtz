using Holtz.DesignPattern.Builder;
using Holtz.DesignPattern.Builder.ConcreteBuilder;
using Holtz.DesignPattern.Builder.Director;
using Holtz.DesignPattern.Builder.Enums;

namespace Holtz.DesignPattern.BuilderTests
{
    public class PizzeriaDirectorTests
    {
        [Fact]
        public void ShoudlCreateAndMountAPizzaPepperoni()
        {
            PizzeriaDirector pizzeriaDirectorForPepperoni = new PizzeriaDirector(new PizzaPepperoni());
            pizzeriaDirectorForPepperoni.MountPizza();
            Pizza pizzaPepperoni = pizzeriaDirectorForPepperoni.GetPizza();
            
            Assert.True(pizzaPepperoni.Size == EnumSize.Big);
            Assert.True(pizzaPepperoni.Edge == EnumPizzaEdge.Normal);
            Assert.True(pizzaPepperoni.DoughType == EnumPizzaDough.Thick);
            Assert.True(pizzaPepperoni.Ingredients.Count == 2);
            Assert.Contains("Pepperoni", pizzaPepperoni.Ingredients);
            Assert.Contains("Tomato", pizzaPepperoni.Ingredients);
        }

        [Fact]
        public void ShoudlCreateAndMountAPizzaBaccon()
        {
            PizzeriaDirector pizzeriaDirectorForBacon = new PizzeriaDirector(new PizzaBacon());
            pizzeriaDirectorForBacon.MountPizza();
            Pizza pizzaBacon = pizzeriaDirectorForBacon.GetPizza();

            Assert.True(pizzaBacon.Size == EnumSize.Small);
            Assert.True(pizzaBacon.Edge == EnumPizzaEdge.Filled);
            Assert.True(pizzaBacon.DoughType == EnumPizzaDough.Normal);
            Assert.True(pizzaBacon.Ingredients.Count == 2);
            Assert.Contains("Bacon", pizzaBacon.Ingredients);
            Assert.Contains("Tomato", pizzaBacon.Ingredients);
        }
    }
}