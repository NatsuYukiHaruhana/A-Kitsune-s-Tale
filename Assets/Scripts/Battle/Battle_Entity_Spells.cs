using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using UnityEngine;

public abstract class Battle_Entity_Spells : ISerializable
{
    private float baseMag;
    private float manaCost;

    private int requiredLevel;
    private string spellText;

    public abstract void PlayAnimation(List<Battle_Entity> targets, List<Battle_Entity> sources);
    public abstract void DealDamage(List<Battle_Entity> targets);
    public abstract void HealTargets(List<Battle_Entity> targets);

    public Battle_Entity_Spells(SerializationInfo info, StreamingContext ctxt) {
        baseMag       = (float) info.GetValue("Base_Magic",     typeof(float));
        manaCost      = (float) info.GetValue("Mana_Cost",      typeof(float));
        requiredLevel = (int)   info.GetValue("Required_Level", typeof(int));
        spellText     = (string)info.GetValue("Spell_Text",     typeof(string));
    }

    public void GetObjectData(SerializationInfo info, StreamingContext ctxt) {
        info.AddValue("Base_Magic", baseMag);
        info.AddValue("Mana_Cost", manaCost);
        info.AddValue("Required_Level", requiredLevel);
        info.AddValue("Spell_Text", spellText);
    }
}
