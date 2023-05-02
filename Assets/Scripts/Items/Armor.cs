using System;
using System.Runtime.Serialization;
using static Battle_Entity_Loadout;

[Serializable()]
public class Armor : Item_Equippable 
{
    protected ArmorSlots armorSlot = ArmorSlots.NULL;

    public Armor(string newName, string newDesc, Battle_Entity_Stats newStats, ArmorSlots newArmorSlot)
        : base(newName, newDesc, newStats) {
        armorSlot = newArmorSlot;
    }

    public Armor(SerializationInfo info, StreamingContext ctxt)
        :base(info, ctxt) {
        armorSlot = (ArmorSlots)info.GetValue("Armor_Slot", typeof(ArmorSlots));
    }

    public new void GetObjectData(SerializationInfo info, StreamingContext ctxt) {
        base.GetObjectData(info, ctxt);
        info.AddValue("Armor_Slot",     armorSlot);
    }
}
