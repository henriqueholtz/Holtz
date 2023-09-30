using Holtz.DesignPattern.Prototype.ConcretePrototype;

namespace Holtz.DesignPattern.Prototype.Tests
{
    public class SoldierTests
    {
        [Fact]
        public void CloneSoldierAsDeepClone()
        {
            Soldier soldier = new Soldier();
            soldier.Name = "Soldier";
            soldier.Weapon = "AK 47";
            soldier.Accessory = new Accessory { Name = "Night display" };

            // Must be deep clone
            Soldier soldierClone = (Soldier)soldier.Clone();
            soldierClone.Name = "Soldier Clone 1";
            soldierClone.Weapon = "L11";
            soldierClone.Accessory.Name = "Bulletproof vest";

            Assert.Equal("Soldier", soldier.Name);
            Assert.Equal("Soldier Clone 1", soldierClone.Name);
            Assert.Equal("L11", soldierClone.Weapon);
            Assert.Equal("Bulletproof vest", soldierClone.Accessory.Name);
        }
    }
}