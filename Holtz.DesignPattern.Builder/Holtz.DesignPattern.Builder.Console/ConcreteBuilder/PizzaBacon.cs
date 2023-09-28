using Holtz.DesignPattern.Builder.Builder;
using Holtz.DesignPattern.Builder.Enums;

namespace Holtz.DesignPattern.Builder.ConcreteBuilder
{
    public sealed class PizzaBacon : PizzaBuilder
    {
        public override void AddIngredients()
        {
            pizza.Ingredients.AddRange(new List<string> { "Bacon", "Tomato" });
        }

        public override void PrepareDough()
        {
            pizza.DoughType = EnumPizzaDough.Normal;
            pizza.Edge = EnumPizzaEdge.Filled; 
            pizza.Size = EnumSize.Small;
        }
    }
}
