using Holtz.DesignPattern.Bridge.ConcreteImplementor;
using Holtz.DesignPattern.Bridge.Domain;

namespace Holtz.DesignPattern.Bridge.Tests
{
    [Collection("Order 3")]
    public class GenerateXmlTests
    {
        [Fact]
        public async Task ShouldExportAsXml()
        {
            string fileName = "employee.xml";
            if (File.Exists(fileName))
                File.Delete(fileName);

            GenerateXml generateJson = new GenerateXml();
            Employee employee = new Employee
            {
                Id = 1,
                Name = "Henrique",
                IncomeBase = 7500,
                Incentivation = 400
            };

            Assert.False(File.Exists(fileName));

            generateJson.GenerateFile(employee);

            Assert.True(File.Exists(fileName));

            await Task.Delay(1000);
        }
    }
}