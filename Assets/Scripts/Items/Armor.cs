using System;
using static Battle_Entity_Loadout;

[Serializable()]
public class Armor : Item_Equippable 
{
    protected ArmorSlots armorSlot = ArmorSlots.NULL;

    public Armor(string newName, string newDesc, Battle_Entity_Stats newStats, ArmorSlots newArmorSlot)
        : base(newName, newDesc, newStats) {
        armorSlot = newArmorSlot;
    }
}
