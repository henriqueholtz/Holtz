// See https://aka.ms/new-console-template for more information
using Holtz.DesignPattern.Prototype.ConcretePrototype;

Console.WriteLine("Hello, Starting Deep clone...");

Soldier soldier = new Soldier();
soldier.Name = "Soldier";
soldier.Weapon = "AK 47";
soldier.Accessory = new Accessory { Name = "Night display" };

// Clone 1 from original soldier
Soldier soldierClone1 = (Soldier)soldier.Clone();
soldierClone1.Name = "Soldier Clone 1";
soldierClone1.Weapon = "L11";
soldierClone1.Accessory.Name = "Bulletproof vest";

// Clone 2 from original soldier
Soldier soldierClone2 = (Soldier)soldier.Clone();
soldierClone2.Name = "Soldier Clone 2";
soldierClone2.Weapon = "P90";
soldierClone2.Accessory.Name = "Bulletproof vest blue";

Console.WriteLine($"Original => {soldier.Name} - {soldier.Weapon} - {soldier.Accessory.Name} \n");
Console.WriteLine($"Clone 1 => {soldierClone1.Name} - {soldierClone1.Weapon} - {soldierClone1.Accessory.Name} \n");
Console.WriteLine($"Clone 2 => {soldierClone2.Name} - {soldierClone2.Weapon} - {soldierClone2.Accessory.Name} \n");