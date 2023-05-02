using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable()]
public class No_Shield : Shield
{
    public No_Shield() : base("---", 
                                "No equipment.",
                                new Battle_Entity_Stats(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0),
                                Battle_Entity_Loadout.HandSlots.NULL) {}

    public No_Shield(SerializationInfo info, StreamingContext ctxt) : base(info, ctxt) {}
}
