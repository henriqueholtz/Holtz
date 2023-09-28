using Holtz.DesignPattern.AbstractFactory.Domain.Enums;

namespace Holtz.DesignPattern.AbstractFactory.Domain.Products
{
    public abstract class Cake : DoughBase
    {
        public Cake(string name, EnumDough dough) : base(name, dough)
        {
        }
    }
}
