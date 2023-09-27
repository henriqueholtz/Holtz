namespace Holtz.DesignPattern.SimpleFactory
{
    public class PepperoniPizza : Pizza
    {
        public PepperoniPizza()
        {
            Name = "Pepperoni Pizza";
        }
        public override string Bake()
        {
            return "Baking a pepperoni pizza...";
        }

        public override string Pack()
        {
            return "Packing a pepperoni pizza...";
        }

        public override string Prepare()
        {
            return "Preparing a pepperoni pizza...";
        }
    }
}
