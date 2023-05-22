using System;

[Serializable()]
public class No_Accessory : Accessory
{
    public No_Accessory() : base("---",
                                    "No equipment.",
                                    new Battle_Entity_Stats(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0),
                                    Battle_Entity_Loadout.AccessorySlots.NULL) {}
}
