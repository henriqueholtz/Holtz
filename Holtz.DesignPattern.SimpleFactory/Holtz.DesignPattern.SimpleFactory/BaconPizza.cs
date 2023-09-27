namespace Holtz.DesignPattern.SimpleFactory
{
    public class BaconPizza : Pizza
    {
        public BaconPizza()
        {
            Name = "Bacon Pizza";
        }
        public override string Bake()
        {
            return "Baking a bacon pizza...";
        }

        public override string Pack()
        {
            return "Packing a bacon pizza...";
        }

        public override string Prepare()
        {
            return "Preparing a bacon pizza...";
        }
    }
}
