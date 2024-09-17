using Holtz.DesignPattern.Adapter.Adapter;
using Holtz.DesignPattern.Adapter.Target;

namespace Holtz.DesignPattern.Adapter.Tests
{
    public class StudentAdapterTests
    {
        [Fact]
        public void ShoudlConvertWithAdapter()
        {
            //arrange
            string[,] studentsArray = new string[5, 4]
            {
                { "101", "Maria", "Artes", "1000" },
                { "102", "Pedro", "Artes", "2000" },
                { "103", "Bianca", "Artes", "3000" },
                { "104", "Pamela", "Artes", "4000" },
                { "105", "Sergio", "Artes", "5000" },
            };
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            //act
            ITarget target = new StudentAdapter();
            target.ProccessTuitionCalc(studentsArray);

            //assert
            string output = stringWriter.ToString();
            Assert.Contains($"Student Maria - Tuition value R$ 1000", output);
            Assert.Contains($"Student Pedro - Tuition value R$ 2000", output);
            Assert.Contains($"Student Bianca - Tuition value R$ 3000", output);
            Assert.Contains($"Student Pamela - Tuition value R$ 4000", output);
            Assert.Contains($"Student Sergio - Tuition value R$ 5000", output);
        }
    }
}