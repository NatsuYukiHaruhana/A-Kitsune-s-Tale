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
    protected string spellText;

    public Battle_Entity_Spells() {
        baseMag = manaCost = 0.0f;
        requiredLevel = 0;
        spellName = spellText = "null";
    }

    public Battle_Entity_Spells(float baseMag, float manaCost, int requiredLevel, string spellText, string spellName) { 
        this.baseMag = baseMag;
        this.manaCost = manaCost;
        this.requiredLevel = requiredLevel;
        this.spellText = spellText;
        this.spellName = spellName;
    }

    public abstract void PlayAnimation(List<Battle_Entity> targets, List<Battle_Entity> sources);
    public abstract void CastSpell(List<Battle_Entity> targets, Battle_Entity caster);

    public string GetSpellName() {
        return spellName;
    }
}
