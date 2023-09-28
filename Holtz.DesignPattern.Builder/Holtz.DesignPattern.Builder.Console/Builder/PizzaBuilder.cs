namespace Holtz.DesignPattern.Builder.Builder
{
    public abstract class PizzaBuilder
    {
        protected Pizza pizza = null!;
        public void CreatePizza()
        {
            pizza = new Pizza();
        }
        public Pizza GetPizza() { return pizza; }

        public abstract void PrepareDough();
        public abstract void AddIngredients();
    }
}
