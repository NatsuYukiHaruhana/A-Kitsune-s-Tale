using System;

[Serializable()]
public class No_Armor : Armor
{
    public No_Armor() : base("---",
                                "No equipment",
                                new Battle_Entity_Stats(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0),
                                Battle_Entity_Loadout.ArmorSlots.NULL) {}
}
