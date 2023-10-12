namespace Holtz.Dapper.Domain.Entities
{
    public class Student
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public bool IsActive { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
