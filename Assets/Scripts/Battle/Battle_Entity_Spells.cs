using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable()]
public abstract class Battle_Entity_Spells
{
    private float baseMag;
    private float manaCost;

    private int requiredLevel;
    private string spellText;

    public abstract void PlayAnimation(List<Battle_Entity> targets, List<Battle_Entity> sources);
    public abstract void DealDamage(List<Battle_Entity> targets);
    public abstract void HealTargets(List<Battle_Entity> targets);
}
