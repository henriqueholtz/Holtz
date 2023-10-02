using Holtz.DesignPattern.Adapter.Domain;

namespace Holtz.DesignPattern.Adapter.Adaptee
{
    public class TuitionSystem
    {
        public void CalcTuition(List<Student> students)
        {
            foreach (Student student in students)
            {
                Console.WriteLine($"Student {student.Name} - Tuition value R$ {student.Tuition}");
            }
        }
    }
}
