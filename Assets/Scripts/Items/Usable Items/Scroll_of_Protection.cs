using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Battle_Entity_Loadout;

[Serializable()]
public class Scroll_of_Protection : Item_Not_Equippable {
    public Scroll_of_Protection()
        : base("Scroll of Protection", "Grants 10 def and res for 2 turns", new Battle_Entity_Stats(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 10), "Audio/SFX/StatBuff", "Animations/Effects/ProtectionUp") { }

    override public void UseItem(Battle_Entity target) {
        Battle_Entity_Stat_Change statChange = new Battle_Entity_Stat_Change(statModifiers, Battle_Entity_Stat_Change.StatChangeType.Buff, 2, target);

        target.AddStatChange(statChange);

        PlaySound(target.GetSFXManager());
        PlayAnimation(target.GetEffectAnimator());
    }
}
