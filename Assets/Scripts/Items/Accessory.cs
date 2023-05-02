using System;
using System.Runtime.Serialization;
using static Battle_Entity_Loadout;

[Serializable()]
public class Accessory : Item_Equippable
{
    protected AccessorySlots accessorySlot = AccessorySlots.NULL;

    public Accessory(string newName, string newDesc, Battle_Entity_Stats newStats, AccessorySlots newAccessorySlot)
        : base(newName, newDesc, newStats) {
        accessorySlot = newAccessorySlot;
    }

    public Accessory(SerializationInfo info, StreamingContext ctxt)
        : base(info, ctxt) {
        accessorySlot = (AccessorySlots)info.GetValue("Accessory_Slot", typeof(AccessorySlots));
    }

    public new void GetObjectData(SerializationInfo info, StreamingContext ctxt) {
        base.GetObjectData(info, ctxt);
        info.AddValue("Accessory_Slot", accessorySlot);
    }

    public AccessorySlots GetAccessorySlots() {
        return accessorySlot;
    }
}
