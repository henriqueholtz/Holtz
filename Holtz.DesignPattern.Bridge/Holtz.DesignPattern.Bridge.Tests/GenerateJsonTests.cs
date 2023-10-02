using Holtz.DesignPattern.Bridge.ConcreteImplementor;
using Holtz.DesignPattern.Bridge.Domain;

namespace Holtz.DesignPattern.Bridge.Tests
{
    [Collection("Order 2")]
    public class GenerateJsonTests
    {
        [Fact]
        public async Task ShouldExportAsJson()
        {
            string fileName = "employee.json";
            if (File.Exists(fileName))
                File.Delete(fileName);

            GenerateJson generateJson = new GenerateJson();
            Employee employee = new Employee
            {
                Id = 1,
                Name = "Henrique",
                IncomeBase = 1500,
                Incentivation = 300
            };

            Assert.False(File.Exists(fileName));

            generateJson.GenerateFile(employee);

            Assert.True(File.Exists(fileName));

            await Task.Delay(1000);
        }
    }
}