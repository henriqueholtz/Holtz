using Holtz.DesignPattern.AbstractFactory.Domain.Enums;

namespace Holtz.DesignPattern.AbstractFactory.Domain.Products
{
    public abstract class Pizza : DoughBase
    {
        protected Pizza(string name, EnumDough dough) : base(name, dough)
        {
        }
    }
}
