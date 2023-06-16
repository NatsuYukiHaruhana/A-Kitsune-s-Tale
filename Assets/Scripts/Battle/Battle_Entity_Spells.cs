using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable()]
public abstract class Battle_Entity_Spells
{
    protected float baseMag;
    protected float manaCost;

    protected int requiredLevel;

    protected string spellName;
    protected string spellDesc;

    protected string spellChars;

    protected Battle_Entity.Faction allowedTargets;

    protected string sFX;
    protected string animatorController;

    public Battle_Entity_Spells() {
        baseMag = manaCost = 0.0f;
        requiredLevel = 0;
        spellName = spellDesc = "null";
    }

    public Battle_Entity_Spells(float baseMag, 
                            float manaCost, 
                            int requiredLevel, 
                            string spellDesc, 
                            string spellName, 
                            string spellChars,
                            Battle_Entity.Faction allowedTargets,
                            string sFX,
                            string animatorController) { 
        this.baseMag = baseMag;
        this.manaCost = manaCost;
        this.requiredLevel = requiredLevel;
        this.spellDesc = spellDesc;
        this.spellName = spellName;
        this.spellChars = spellChars;
        this.allowedTargets = allowedTargets;
        this.sFX = sFX;
        this.animatorController = animatorController;
    }

    public abstract void PlayAnimation(List<Battle_Entity> targets, List<Battle_Entity> sources);
    public abstract void CastSpell(List<Battle_Entity> targets, Battle_Entity caster);
    public void PlaySound(Sound_Manager sfxManager) {
        sfxManager.PlaySound(Resources.Load(sFX) as AudioClip);
    }

    public string GetSpellName() {
        return spellName;
    }

    public string GetSpellDesc() {
        return spellDesc;
    }

    public string GetSpellChars() {
        return spellChars;
    }

    public Battle_Entity.Faction GetAllowedTargets() {
        return allowedTargets;
    }
}
