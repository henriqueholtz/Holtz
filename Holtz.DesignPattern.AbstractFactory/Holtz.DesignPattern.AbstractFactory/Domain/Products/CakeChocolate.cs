using Holtz.DesignPattern.AbstractFactory.Domain.Enums;

namespace Holtz.DesignPattern.AbstractFactory.Domain.Products
{
    public sealed class CakeChocolate : Cake
    {
        public CakeChocolate() : base("Cake of chocolate", EnumDough.Cake)
        {
            Ingredients.Add("Sugar");
            Ingredients.Add("Chocolate");
        }
    }
}
