using Holtz.DesignPattern.AbstractFactory.Domain.Enums;
using System.Collections;

namespace Holtz.DesignPattern.AbstractFactory.Domain.Products
{
    public abstract class DoughBase
    {
        public EnumDough Dough { get; set; }
        public string Name { get; set; }
        public ArrayList Ingredients { get; set; } = new ArrayList();
        public DoughBase(string name, EnumDough dough)
        {
            Name = name;
            Dough = dough;
        }
    }
}
