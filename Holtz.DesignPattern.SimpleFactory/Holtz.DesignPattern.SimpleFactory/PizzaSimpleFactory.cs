namespace Holtz.DesignPattern.SimpleFactory
{
    /// <summary>
    /// PizzaSimpleFactory cannot be inherited (because it's a sealed class)
    /// </summary>
    public sealed class PizzaSimpleFactory
    {
        public static Pizza CreatePizza(string type)
        {
            Pizza pizza;

            switch(type.ToUpper())
            {
                case "BACON":
                    pizza = new BaconPizza();
                    break;
                case "PEPPERONI":
                    pizza = new PepperoniPizza();
                    break;
                default:
                    throw new ApplicationException("Pizza type unknown/invalid!");
            }

            return pizza;
        }
    }
}
