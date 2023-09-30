namespace Holtz.DesignPattern.Prototype.ConcretePrototype
{
    public class Accessory : ICloneable
    {
        public string Name { get; set; } = null!;
        public object Clone()
        {
            return (Accessory)this.MemberwiseClone();
        }
    }
}
