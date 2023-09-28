// See https://aka.ms/new-console-template for more information
using Holtz.DesignPattern.Builder.ConcreteBuilder;
using Holtz.DesignPattern.Builder.Director;

Console.WriteLine("Hello, Starting...");
Console.WriteLine("\n\n");

// Director
var pizzeriaDirectorForPepperoni = new PizzeriaDirector(new PizzaPepperoni());
pizzeriaDirectorForPepperoni.MountPizza();
var pizzaPepperoni = pizzeriaDirectorForPepperoni.GetPizza();
pizzaPepperoni.ShowContent();


var pizzeriaDirectorForBacon = new PizzeriaDirector(new PizzaBacon());
pizzeriaDirectorForBacon.MountPizza();
var pizzaBacon = pizzeriaDirectorForBacon.GetPizza();
pizzaBacon.ShowContent();

Console.ReadLine();