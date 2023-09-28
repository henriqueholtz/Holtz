using Holtz.DesignPattern.Builder.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Holtz.DesignPattern.Builder.Director
{
    public class PizzeriaDirector
    {
        private readonly PizzaBuilder _builder;
        public PizzeriaDirector(PizzaBuilder pizzaBuilder)
        {
            _builder = pizzaBuilder;
        }

        public void MountPizza()
        {
            _builder.CreatePizza();
            _builder.PrepareDough();
            _builder.AddIngredients();
        }

        public Pizza GetPizza()
        {
            return _builder.GetPizza();
        }
    }
}
