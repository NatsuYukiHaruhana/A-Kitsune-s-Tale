using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[Serializable()]
public class Lightning : Battle_Entity_Spells {

    public Lightning() :
        base(20,
            10,
            1,
            "The user calls upon the power of electricity to call forth a great bolt of lightning upon their enemy.",
            "Lightning",
            "電",
            Battle_Entity.Faction.Enemy,
            "Audio/SFX/Lightning",
            "Animations/Spells/Lightning") { }

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

        spellPrefab.transform.position = targets[0].transform.position + new Vector3(0.0f, 0.5f);

        animator.runtimeAnimatorController = Resources.Load(animatorController) as RuntimeAnimatorController;
        animator.enabled = true;
    }
}
