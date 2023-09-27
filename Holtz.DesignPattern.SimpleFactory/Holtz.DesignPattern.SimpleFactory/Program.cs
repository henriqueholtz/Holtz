// See https://aka.ms/new-console-template for more information
using Holtz.DesignPattern.SimpleFactory;

Console.WriteLine("Hello, Welcome to Holtz pizzeria!");

Console.WriteLine("You can choose your pizza between the following options: ");
Console.WriteLine("=> BACON");
Console.WriteLine("=> PEPPERONI");
Console.Write("Please type you chosen: ");

string? type = Console.ReadLine();
Console.WriteLine();
try
{
    Pizza pizza = PizzaSimpleFactory.CreatePizza(type ?? "");
    Console.WriteLine(pizza.Prepare());
    Console.WriteLine(pizza.Bake());
    Console.WriteLine(pizza.Pack());
    Console.WriteLine($"{pizza.Name} ready!");
}
catch(Exception ex)
{
    Console.WriteLine($"Error: {ex}");
}

Console.ReadLine();