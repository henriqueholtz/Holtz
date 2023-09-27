namespace Holtz.DesignPattern.SimpleFactory
{
    public class PepperoniPizza : Pizza
    {
        public PepperoniPizza()
        {
            Name = "Pepperoni Pizza";
        }
        public override void Bake()
        {
            Console.WriteLine("Baking a pepperoni pizza...");
        }

        public override void Pack()
        {
            Console.WriteLine("Packing a pepperoni pizza...");
        }

        public override void Prepare()
        {
            Console.WriteLine("Preparing a pepperoni pizza...");
        }
    }
}
