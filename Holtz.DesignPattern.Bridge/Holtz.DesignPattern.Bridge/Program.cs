// See https://aka.ms/new-console-template for more information
using Holtz.DesignPattern.Bridge.ConcreteImplementor;
using Holtz.DesignPattern.Bridge.Domain;
using Holtz.DesignPattern.Bridge.RefinedAbstraction;

Console.WriteLine("Hello, Starting...");


CalcIncome calcIncome = new CalcIncome(new GenerateXml());

Employee employee = new Employee
{
    Id = 1,
    Name = "Henrique",
    IncomeBase = 11000,
    Incentivation = 1500
};

calcIncome.ProccessIncomeEmployee(employee);

employee.Incentivation = 2500;

calcIncome = new CalcIncome(new GenerateJson());

calcIncome.ProccessIncomeEmployee(employee);
Console.ReadLine();