using System;
using System.Runtime.Serialization;

[Serializable()]
public class No_Armor : Armor
{
    public No_Armor() : base("---",
                                "No equipment",
                                new Battle_Entity_Stats(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0),
                                Battle_Entity_Loadout.ArmorSlots.NULL) {}

    public No_Armor(SerializationInfo info, StreamingContext ctxt) : base(info, ctxt) {}
}
