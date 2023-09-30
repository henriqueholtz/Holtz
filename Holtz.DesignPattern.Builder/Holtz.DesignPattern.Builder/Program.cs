// See https://aka.ms/new-console-template for more information
using Holtz.DesignPattern.Builder;
using Holtz.DesignPattern.Builder.ConcreteBuilder;
using Holtz.DesignPattern.Builder.Director;

Console.WriteLine("Hello, Starting...");
Console.WriteLine("\n\n");

// Director
PizzeriaDirector pizzeriaDirectorForPepperoni = new PizzeriaDirector(new PizzaPepperoni());
pizzeriaDirectorForPepperoni.MountPizza();
Pizza pizzaPepperoni = pizzeriaDirectorForPepperoni.GetPizza();
pizzaPepperoni.ShowContent();


PizzeriaDirector pizzeriaDirectorForBacon = new PizzeriaDirector(new PizzaBacon());
pizzeriaDirectorForBacon.MountPizza();
Pizza pizzaBacon = pizzeriaDirectorForBacon.GetPizza();
pizzaBacon.ShowContent();

Console.ReadLine();