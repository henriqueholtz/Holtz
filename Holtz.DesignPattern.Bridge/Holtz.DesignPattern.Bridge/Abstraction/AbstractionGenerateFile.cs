using Holtz.DesignPattern.Bridge.Domain;
using Holtz.DesignPattern.Bridge.Implementor;

namespace Holtz.DesignPattern.Bridge.Abstraction
{
    public abstract class AbstractionGenerateFile
    {
        protected IGenerateFile _generateFile;

        protected AbstractionGenerateFile(IGenerateFile generateFile)
        {
            _generateFile = generateFile;
        }

        public virtual void GenerateFile(Employee employee)
        {
            _generateFile.GenerateFile(employee);
        }
    }
}
