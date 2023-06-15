using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Battle_Entity_Loadout;

[Serializable()]
public class Potion : Item_Not_Equippable {
    public Potion() 
        : base("Potion", "Heals 20 HP", new Battle_Entity_Stats(0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0), "Audio/SFX/Heal", "Animations/Effects/Heal") {}

    override public void UseItem(Battle_Entity target) {
        target.Heal(statModifiers.currHP);

        PlaySound(target.GetSFXManager());
        PlayAnimation(target.GetEffectAnimator());
    }
}
