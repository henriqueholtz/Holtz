namespace Holtz.DesignPattern.Bridge.Domain
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal IncomeBase { get; set; }
        public decimal Incentivation { get; set; }
        public decimal IncomeTotal { get; set; }
    }
}
