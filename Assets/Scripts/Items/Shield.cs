using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using static Battle_Entity_Loadout;
using UnityEngine;

[Serializable()]
public class Shield : Item_Equippable
{
    protected HandSlots handSlot = HandSlots.NULL;

    public Shield(string newName, string newDesc, Battle_Entity_Stats newStats, HandSlots newHandSlot)
        : base(newName, newDesc, newStats) {
        handSlot = newHandSlot;
    }

    public Shield(Shield other) : base(other.itemName, other.itemDesc, other.statModifiers) {
        handSlot = other.handSlot;
    }

    public Shield(SerializationInfo info, StreamingContext ctxt)
        : base(info, ctxt) {
        handSlot = (HandSlots)info.GetValue("Hand_Slot", typeof(HandSlots));
    }

    public new void GetObjectData(SerializationInfo info, StreamingContext ctxt) {
        base.GetObjectData(info, ctxt);
        info.AddValue("Hand_Slot",      handSlot);
    }
}
