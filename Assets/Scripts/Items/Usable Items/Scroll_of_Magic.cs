using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Battle_Entity_Loadout;

[Serializable()]
public class Scroll_of_Magic : Item_Not_Equippable {
    public Scroll_of_Magic()
        : base("Scroll of Magic", "Grants 10 mag for 2 turns", new Battle_Entity_Stats(0, 0, 0, 0, 0, 0, 0, 0, 10, 0, 0, 0), "Audio/SFX/StatBuff", "Animations/Effects/MagicUp") { }

    override public void UseItem(Battle_Entity target) {
        Battle_Entity_Stat_Change statChange = new Battle_Entity_Stat_Change(statModifiers, Battle_Entity_Stat_Change.StatChangeType.Buff, 2, target);

        target.AddStatChange(statChange);

        PlaySound(target.GetSFXManager());
        PlayAnimation(target.GetEffectAnimator());
    }
}
