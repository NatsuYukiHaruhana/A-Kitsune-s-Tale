using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Battle_Entity_Loadout;

[Serializable()]
public class Scroll_of_Strength : Item_Not_Equippable {
    public Scroll_of_Strength()
        : base("Scroll of Strength", "Grants 10 str for 2 turns", new Battle_Entity_Stats(0, 0, 0, 0, 0, 0, 0, 10, 0, 0, 0, 0), "Audio/SFX/StatBuff", "Animations/Effects/StrengthUp") { }

    override public void UseItem(Battle_Entity target) {
        Battle_Entity_Stat_Change statChange = new Battle_Entity_Stat_Change(statModifiers, Battle_Entity_Stat_Change.StatChangeType.Buff, 2, target);

        target.AddStatChange(statChange);

        PlaySound(target.GetSFXManager());
        PlayAnimation(target.GetEffectAnimator());
    }
}
