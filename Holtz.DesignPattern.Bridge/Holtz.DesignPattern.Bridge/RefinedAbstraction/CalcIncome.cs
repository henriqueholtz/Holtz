using Holtz.DesignPattern.Bridge.Abstraction;
using Holtz.DesignPattern.Bridge.Domain;
using Holtz.DesignPattern.Bridge.Implementor;

namespace Holtz.DesignPattern.Bridge.RefinedAbstraction
{
    public class CalcIncome : AbstractionGenerateFile
    {
        public CalcIncome(IGenerateFile generateFile) : base(generateFile)
        { }

        public void ProccessIncomeEmployee(Employee employee) 
        { 
            employee.IncomeTotal= employee.IncomeBase + employee.Incentivation;

            Console.WriteLine($"Income Total to employee {employee.Name} is R$: {employee.IncomeTotal}");

            _generateFile.GenerateFile(employee);
        }
    }
}
