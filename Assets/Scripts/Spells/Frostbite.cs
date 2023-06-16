using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[Serializable()]
public class Frostbite : Battle_Entity_Spells {

    public Frostbite() :
        base(20,
            10,
            1,
            "The user calls upon the power of ice to summon crystals which slash the enemy.",
            "Frostbite",
            "氷",
            Battle_Entity.Faction.Enemy,
            "Audio/SFX/Frostbite",
            "Animations/Spells/Frostbite") { }

    public override void CastSpell(List<Battle_Entity> targets, Battle_Entity caster) {
        if (caster.GetStats().currMana < manaCost) {
            return;
        }

        caster.ReduceMana(manaCost);
        targets[0].TakeDamage(caster.GetStats().mag + baseMag, DamageType.Magical);
        PlaySound(caster.GetSFXManager());
        PlayAnimation(targets, new List<Battle_Entity>() { caster });
    }

    public override void PlayAnimation(List<Battle_Entity> targets, List<Battle_Entity> sources) {
        GameObject spellPrefab = GameObject.Instantiate(Resources.Load("Prefabs/Spell") as GameObject);
        Animator animator = null;
        if (spellPrefab != null) {
            animator = spellPrefab.GetComponent<Animator>();
        }

        spellPrefab.transform.position = targets[0].transform.position;

        animator.runtimeAnimatorController = Resources.Load(animatorController) as RuntimeAnimatorController;
        animator.enabled = true;
    }
}
