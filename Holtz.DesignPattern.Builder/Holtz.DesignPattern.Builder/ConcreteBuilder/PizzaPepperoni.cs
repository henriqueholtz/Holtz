using Holtz.DesignPattern.Builder.Builder;
using Holtz.DesignPattern.Builder.Enums;

namespace Holtz.DesignPattern.Builder.ConcreteBuilder
{
    public sealed class PizzaPepperoni : PizzaBuilder
    {
        public override void AddIngredients()
        {
            pizza.Ingredients.AddRange(new List<string> { "Pepperoni", "Tomato" });
        }

        public override void PrepareDough()
        {
            pizza.DoughType = EnumPizzaDough.Thick;
            pizza.Edge = EnumPizzaEdge.Normal; 
            pizza.Size = EnumSize.Big;
        }
    }
}
