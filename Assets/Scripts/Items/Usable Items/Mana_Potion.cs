using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Battle_Entity_Loadout;

[Serializable()]
public class Mana_Potion : Item_Not_Equippable {
    public Mana_Potion()
        : base("Mana Potion", "Restores 20 MP", new Battle_Entity_Stats(0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0), "Audio/SFX/Mana Up", "Animations/Effects/ManaUp") { }

    override public void UseItem(Battle_Entity target) {
        target.RestoreMana(statModifiers.currMana);

        PlaySound(target.GetSFXManager());
        PlayAnimation(target.GetEffectAnimator());
    }
}
