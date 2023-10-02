using Holtz.DesignPattern.Bridge.ConcreteImplementor;
using Holtz.DesignPattern.Bridge.Domain;
using Holtz.DesignPattern.Bridge.RefinedAbstraction;

namespace Holtz.DesignPattern.Bridge.Tests
{
    [Collection("Order 1")]
    public class CalcIncomeTests
    {
        [Fact]
        public async Task ShouldCalculateIncomeCorrectlyAndExportAsXml()
        {
            string fileName = "employee.xml";
            if (File.Exists(fileName))
                File.Delete(fileName);

            CalcIncome calcIncome = new CalcIncome(new GenerateXml());

            Employee employee = new Employee
            {
                Id = 1,
                Name = "Henrique",
                IncomeBase = 11000,
                Incentivation = 1500
            };

            Assert.False(File.Exists(fileName));

            // Delay to avoid conflicts opening files with unit tests from GenerateXmlTests
            await Task.Delay(1100);
            calcIncome.ProccessIncomeEmployee(employee);

            Assert.Equal(12500, employee.IncomeTotal);
            Assert.True(File.Exists(fileName));
        }

        [Fact]
        public async Task ShouldCalculateIncomeCorrectlyAndExportAsJson()
        {
            string fileName = "employee.json";
            if (File.Exists(fileName))
                File.Delete(fileName);

            CalcIncome calcIncome = new CalcIncome(new GenerateJson());

            Employee employee = new Employee
            {
                Id = 1,
                Name = "Henrique",
                IncomeBase = 1500,
                Incentivation = 300
            };

            Assert.False(File.Exists(fileName));

            // Delay to avoid conflicts opening files with unit tests from GenerateJsonTests
            await Task.Delay(1000);
            calcIncome.ProccessIncomeEmployee(employee);

            Assert.Equal(1800, employee.IncomeTotal);
            Assert.True(File.Exists(fileName));
        }
    }
}