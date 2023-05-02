using System;
using System.Runtime.Serialization;

[Serializable()]
public class No_Weapon : Weapon
{
    public No_Weapon() : base("---", 
                                "No equipment.", 
                                new Battle_Entity_Stats(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0), 
                                DamageType.NULL, 
                                Battle_Entity_Loadout.HandSlots.NULL) {}
    public No_Weapon(SerializationInfo info, StreamingContext ctxt) : base(info, ctxt) {}
}
