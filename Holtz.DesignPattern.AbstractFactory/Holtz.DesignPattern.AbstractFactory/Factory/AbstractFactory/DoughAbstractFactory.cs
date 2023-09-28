using Holtz.DesignPattern.AbstractFactory.Domain.Enums;
using Holtz.DesignPattern.AbstractFactory.Domain.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Holtz.DesignPattern.AbstractFactory.Factory.AbstractFactory
{
    public abstract class DoughAbstractFactory
    {
        public abstract DoughBase CreateDough(EnumDough dough);
        public static DoughAbstractFactory CreateFactoryDough(EnumDough dough)
        {
            switch (dough)
            {
                case EnumDough.Pizza:
                    return new FactoryPizza();
                case EnumDough.Cake:
                    return new FactoryCake();
                default:
                    throw new ArgumentOutOfRangeException(nameof(dough), dough, null);

            }
        }
    }
}
