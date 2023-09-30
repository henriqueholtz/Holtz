using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Holtz.DesignPattern.Prototype.ConcretePrototype
{
    public class Soldier : ICloneable
    {
        public string Name { get; set; } = null!;
        public string Weapon { get; set; } = null!;
        public Accessory Accessory { get; set; }
        public Soldier() { }
        public Soldier(Soldier soldier)
        {
            this.Accessory = soldier.Accessory;
            this.Weapon = soldier.Weapon;
            this.Name = soldier.Name;
        }
        public object Clone()
        {
            // Shallow copy (only native types like string, int, etc)
            //return new Soldier(this);

            // Deep clone
            Soldier clone = (Soldier)this.MemberwiseClone();
            clone.Accessory = (Accessory)this.Accessory.Clone();
            return clone;
        }
    }
}
