using Holtz.DesignPattern.Bridge.Domain;
using Holtz.DesignPattern.Bridge.Implementor;
using System.Text.Json;

namespace Holtz.DesignPattern.Bridge.ConcreteImplementor
{
    public class GenerateJson : IGenerateFile
    {
        private const string FILE_NAME = "employee.json";
        public void GenerateFile(Employee employee)
        {
            string employeeSerialized = JsonSerializer.Serialize(employee);

            File.WriteAllText(FILE_NAME, employeeSerialized);

            Console.WriteLine($"Income of employee {employee.Name} has ben generated successfully at {FILE_NAME}");
        }
    }
}
