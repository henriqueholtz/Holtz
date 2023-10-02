using Holtz.DesignPattern.Bridge.Domain;
using Holtz.DesignPattern.Bridge.Implementor;
using System.Xml.Serialization;

namespace Holtz.DesignPattern.Bridge.ConcreteImplementor
{
    public class GenerateXml : IGenerateFile
    {
        private const string FILE_NAME = "employee.xml";
        private XmlSerializer _serializer = new XmlSerializer(typeof(Employee));
        public void GenerateFile(Employee employee)
        {
            using (FileStream fileStream = new FileStream(FILE_NAME, FileMode.OpenOrCreate))
            {
                _serializer.Serialize(fileStream, employee);
            }

            Console.WriteLine($"Income of employee {employee.Name} has ben generated successfully at {FILE_NAME}");
        }
    }
}
