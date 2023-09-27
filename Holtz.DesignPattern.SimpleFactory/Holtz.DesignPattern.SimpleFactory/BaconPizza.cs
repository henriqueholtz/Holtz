namespace Holtz.DesignPattern.SimpleFactory
{
    public class BaconPizza : Pizza
    {
        public BaconPizza()
        {
            Name = "Bacon Pizza";
        }
        public override void Bake()
        {
            Console.WriteLine("Baking a bacon pizza...");
        }

        public override void Pack()
        {
            Console.WriteLine("Packing a bacon pizza...");
        }

        public override void Prepare()
        {
            Console.WriteLine("Preparing a bacon pizza...");
        }
    }
}
