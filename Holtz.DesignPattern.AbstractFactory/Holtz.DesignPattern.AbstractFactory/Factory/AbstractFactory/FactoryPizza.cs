using Holtz.DesignPattern.AbstractFactory.Domain.Enums;
using Holtz.DesignPattern.AbstractFactory.Domain.Products;

namespace Holtz.DesignPattern.AbstractFactory.Factory.AbstractFactory
{
    public sealed class FactoryPizza : DoughAbstractFactory
    {
        public override DoughBase CreateDough(EnumDough dough)
        {
            EnumPizza pizzaType = (EnumPizza)dough;

            switch (pizzaType)
            {
                case EnumPizza.Pepperoni:
                    return new PizzaPepperoni();
                case EnumPizza.Bacon:
                    return new PizzaBacon();
                default:
                    throw new ArgumentOutOfRangeException("Invalid pizza type!");
            }
        }
    }
}
