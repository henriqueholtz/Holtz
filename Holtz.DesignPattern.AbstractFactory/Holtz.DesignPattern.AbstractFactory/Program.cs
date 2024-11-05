using Holtz.DesignPattern.AbstractFactory.Domain.Enums;
using Holtz.DesignPattern.AbstractFactory.Domain.Products;
using Holtz.DesignPattern.AbstractFactory.Factory.AbstractFactory;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Holtz.DesignPattern.AbstractFactoryTests")]
internal class Program
{
    public static void Main(string[] args)
    {
        static void ShowDetails(DoughBase doughBase) => Console.WriteLine($"Type is {doughBase.Name}. Ingredients: {string.Join(",", doughBase.Ingredients.ToArray())}...");

        Console.WriteLine("Hello, Starting...");

        // Get Factories
        DoughAbstractFactory factoryCake = DoughAbstractFactory.CreateFactoryDough(EnumDough.Cake);
        DoughAbstractFactory factoryPizza = DoughAbstractFactory.CreateFactoryDough(EnumDough.Pizza);

        // Create objects based on cake's type
        DoughBase cakeChocolate = factoryCake.CreateDough((EnumDough)EnumCake.Chocolate);
        DoughBase cakeOrange = factoryCake.CreateDough((EnumDough)EnumCake.Orange);

        // Create objects based on pizza's type
        DoughBase pizzaPepperoni = factoryPizza.CreateDough((EnumDough)EnumPizza.Pepperoni);
        DoughBase pizzaBacon = factoryPizza.CreateDough((EnumDough)EnumPizza.Bacon);

        // Show details
        ShowDetails(cakeChocolate);
        ShowDetails(cakeOrange);
        ShowDetails(pizzaPepperoni);
        ShowDetails(pizzaBacon);

        Console.WriteLine("Finished.");
    }
}