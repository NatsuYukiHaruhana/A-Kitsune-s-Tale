using System;
using static Battle_Entity_Loadout;

[Serializable()]
public class Weapon : Item_Equippable
{
    protected DamageType damageType = DamageType.NULL;
    protected HandSlots handSlot = HandSlots.NULL;

    public Weapon(string newName, string newDesc, Battle_Entity_Stats newStats, 
                            DamageType newDamageType, HandSlots newHandSlot)
        : base(newName, newDesc, newStats) {
        damageType = newDamageType;
        handSlot = newHandSlot;
    }

    public DamageType GetDamageType() {
        return damageType;
    }
}
