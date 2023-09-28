using Holtz.DesignPattern.AbstractFactory.Domain.Enums;

namespace Holtz.DesignPattern.AbstractFactory.Domain.Products
{
    public sealed class CakeOrange : Cake
    {
        public CakeOrange() : base("Cake of orange", EnumDough.Cake)
        {
            Ingredients.Add("Sugar");
            Ingredients.Add("Orange");
        }
    }
}
