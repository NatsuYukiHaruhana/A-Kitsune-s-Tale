using System;
using System.Runtime.Serialization;

[Serializable()]
public class Bare_Handed : Weapon
{
    public Bare_Handed() : base("Bare-Handed", 
                                "Perhaps showing up bare-handed wasn't a good idea...",
                                new Battle_Entity_Stats(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0),
                                DamageType.Physical,
                                Battle_Entity_Loadout.HandSlots.Weapon_Both) {}

    public Bare_Handed(SerializationInfo info, StreamingContext ctxt) : base(info, ctxt) {}
}
