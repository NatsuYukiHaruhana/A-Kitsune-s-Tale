using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Battle_Entity_Loadout;

[Serializable()]
public abstract class Item_Not_Equippable : Item {
    protected Battle_Entity_Stats statModifiers;
    protected string sFX;
    protected string animatorController;

    public Item_Not_Equippable() { }

    public Item_Not_Equippable(string newName, string newDesc, Battle_Entity_Stats newStats, string newSFX, string animatorController) {
        itemName = newName;
        itemDesc = newDesc;
        statModifiers = newStats;
        sFX = newSFX;
        this.animatorController = animatorController;
    }

    abstract public void UseItem(Battle_Entity target);

    public void PlaySound(Sound_Manager sfxManager) {
        sfxManager.PlaySound(Resources.Load(sFX) as AudioClip);
    }

    public void PlayAnimation(Animator unitEffectAnimation) {
        unitEffectAnimation.runtimeAnimatorController = Resources.Load(animatorController) as RuntimeAnimatorController;
        unitEffectAnimation.enabled = true;
    }

    public Battle_Entity_Stats GetStats() {
        return statModifiers;
    }
}
