using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using static Battle_Entity_Loadout;

[Serializable()]
public abstract class Item_Equippable : Item
{
    protected Battle_Entity_Stats statModifiers;

    public Item_Equippable() {}

    public Item_Equippable(string newName, string newDesc, Battle_Entity_Stats newStats) {
        itemName = newName;
        itemDesc = newDesc;
        statModifiers = newStats;
    }

    public Item_Equippable(SerializationInfo info, StreamingContext ctxt)
        :base(info, ctxt) {
        statModifiers = (Battle_Entity_Stats)info.GetValue("Stat_Modifiers", typeof(Battle_Entity_Stats));
    }

    public Battle_Entity_Stats GetStats() {
        return statModifiers;
    }

    public new void GetObjectData(SerializationInfo info, StreamingContext ctxt) {
        base.GetObjectData(info, ctxt);
        info.AddValue("Stat_Modifiers", statModifiers);
    }
}
