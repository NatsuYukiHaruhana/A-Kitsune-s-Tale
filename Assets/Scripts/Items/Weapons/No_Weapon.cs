using System;

[Serializable()]
public class No_Weapon : Weapon
{
    public No_Weapon() : base("---", 
                                "No equipment.", 
                                new Battle_Entity_Stats(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0), 
                                DamageType.NULL, 
                                Battle_Entity_Loadout.HandSlots.NULL) {}
}
