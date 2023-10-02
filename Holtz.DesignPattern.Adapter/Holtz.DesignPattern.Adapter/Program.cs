// See https://aka.ms/new-console-template for more information
using Holtz.DesignPattern.Adapter.Adapter;
using Holtz.DesignPattern.Adapter.Target;

Console.WriteLine("Hello, Starting...");

string[,] studentsArray = new string[5, 4]
{
    { "101", "Maria", "Artes", "1000" },
    { "102", "Pedro", "Artes", "2000" },
    { "103", "Bianca", "Artes", "3000" },
    { "104", "Pamela", "Artes", "4000" },
    { "105", "Sergio", "Artes", "5000" },
};

ITarget target = new StudentAdapter();

target.ProccessTuitionCalc(studentsArray);

Console.WriteLine();
Console.ReadLine();
