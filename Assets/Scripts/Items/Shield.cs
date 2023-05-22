using System;
using System.Diagnostics;
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
}
