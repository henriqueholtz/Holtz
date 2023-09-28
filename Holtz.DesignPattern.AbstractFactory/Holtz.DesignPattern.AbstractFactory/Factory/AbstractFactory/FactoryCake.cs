using Holtz.DesignPattern.AbstractFactory.Domain.Enums;
using Holtz.DesignPattern.AbstractFactory.Domain.Products;

namespace Holtz.DesignPattern.AbstractFactory.Factory.AbstractFactory
{
    public sealed class FactoryCake : DoughAbstractFactory
    {
        public override DoughBase CreateDough(EnumDough dough)
        {
            EnumCake cakeType = (EnumCake)dough;

            switch (cakeType)
            {
                case EnumCake.Chocolate:
                    return new CakeChocolate();
                case EnumCake.Orange:
                    return new CakeOrange();
                default:
                    throw new ArgumentOutOfRangeException("Invalid cake type!");
            }
        }
    }
}
