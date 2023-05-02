using System;
using System.Runtime.Serialization;
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

    public Weapon(SerializationInfo info, StreamingContext ctxt)
        :base(info, ctxt) {
        damageType    = (DamageType)info.GetValue("Damage_Type", typeof(DamageType));
        handSlot      = (HandSlots) info.GetValue("Hand_Slot",   typeof(HandSlots));
    }

    public new void GetObjectData(SerializationInfo info, StreamingContext ctxt) {
        base.GetObjectData(info, ctxt);
        info.AddValue("Damage_Type",    damageType);
        info.AddValue("Hand_Slot",      handSlot);
    }

    public DamageType GetDamageType() {
        return damageType;
    }
}
