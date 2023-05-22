using System;
using static Battle_Entity_Loadout;

[Serializable()]
public class Accessory : Item_Equippable
{
    protected AccessorySlots accessorySlot = AccessorySlots.NULL;

    public Accessory(string newName, string newDesc, Battle_Entity_Stats newStats, AccessorySlots newAccessorySlot)
        : base(newName, newDesc, newStats) {
        accessorySlot = newAccessorySlot;
    }

    public AccessorySlots GetAccessorySlots() {
        return accessorySlot;
    }
}
