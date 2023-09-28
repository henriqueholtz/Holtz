using Holtz.DesignPattern.Builder.Enums;

namespace Holtz.DesignPattern.Builder
{
    public class Pizza
    {
        public EnumPizzaDough DoughType { get; set; }
        public EnumPizzaEdge Edge { get; set; }
        public EnumSize Size { get; set; }
        public List<string> Ingredients { get; set; } = new List<string>();

        public void ShowContent()
        {
            Console.WriteLine($"Pizza with dough {DoughType}, Edge: {Edge}, Size: {Size}. Ingredients: {string.Join(", ", Ingredients)}");
            Console.WriteLine("\n");
        }
    }
}
