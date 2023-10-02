
namespace Holtz.DesignPattern.Adapter.Domain
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Course { get; set; } = null!;
        public decimal Tuition { get; set; }
        public Student(int id, string name, string course, decimal tuition)
        {
            Id = id;
            Name = name;
            Course = course;
            Tuition = tuition;
        }
    }
}
