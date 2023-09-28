// See https://aka.ms/new-console-template for more information
using Holtz.DesignPattern.AbstractFactory.Domain.Enums;
using Holtz.DesignPattern.AbstractFactory.Domain.Products;
using Holtz.DesignPattern.AbstractFactory.Factory.AbstractFactory;

static void ShowDetails(DoughBase doughBase) => Console.WriteLine($"Type is {doughBase.Name}. Name is {doughBase.Name}. Ingredients: {string.Join(",", doughBase.Ingredients.ToArray())}... \n");

Console.WriteLine("Hello, Starting...");
Console.WriteLine();
Console.WriteLine();

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

Console.WriteLine();
Console.WriteLine("Finished.");
Console.ReadLine();