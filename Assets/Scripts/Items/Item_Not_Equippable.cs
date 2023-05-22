using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Battle_Entity_Loadout;

[Serializable()]
public abstract class Item_Not_Equippable : Item {
    protected Battle_Entity_Stats statModifiers;

    public Item_Not_Equippable() { }

    public Item_Not_Equippable(string newName, string newDesc, Battle_Entity_Stats newStats) {
        itemName = newName;
        itemDesc = newDesc;
        statModifiers = newStats;
    }

    public Battle_Entity_Stats GetStats() {
        return statModifiers;
    }
}
