using Holtz.DesignPattern.Adapter.Adaptee;
using Holtz.DesignPattern.Adapter.Domain;
using Holtz.DesignPattern.Adapter.Target;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Holtz.DesignPattern.Adapter.Adapter
{
    public class StudentAdapter : ITarget
    {
        private TuitionSystem _tuitionSystem = new TuitionSystem();
        public void ProccessTuitionCalc(string[,] studentsArray)
        {
            string id = "", name = "", course = "", tuition = "";

            List<Student> studentsList = new List<Student>();
            for (int i = 0; i <studentsArray.GetLength(0); i++)
            {
                for (int j = 0; j < studentsArray.GetLength(1); j++)
                {
                    if (j == 0)
                    {
                        id = studentsArray[i, j];
                    }
                    else if (j == 1)
                    {
                        name = studentsArray[i, j];
                    }
                    else if (j ==1)
                    {
                        course = studentsArray[i, j];
                    } else
                    {
                         tuition = studentsArray[i, j];
                    }
                }
                studentsList.Add(new Student(Convert.ToInt32(id), name, course, Convert.ToDecimal(tuition)));
            }
            Console.WriteLine("Converted!");

            _tuitionSystem.CalcTuition(studentsList);
        }
    }
}
