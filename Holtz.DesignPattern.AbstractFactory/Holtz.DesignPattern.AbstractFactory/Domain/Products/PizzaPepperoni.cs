using Holtz.DesignPattern.AbstractFactory.Domain.Enums;

namespace Holtz.DesignPattern.AbstractFactory.Domain.Products
{
    public sealed class PizzaPepperoni : Pizza
    {
        public PizzaPepperoni() : base("Pizza of Pepperoni", EnumDough.Pizza)
        {
            Ingredients.Add("Pepperoni");
        }
    }
}
