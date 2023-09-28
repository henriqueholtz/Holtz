using Holtz.DesignPattern.AbstractFactory.Domain.Enums;

namespace Holtz.DesignPattern.AbstractFactory.Domain.Products
{
    public sealed class PizzaBacon : Pizza
    {
        public PizzaBacon() : base("Pizza of Bacon", EnumDough.Pizza)
        {
            Ingredients.Add("Bacon");
        }
    }
}
