using System;
using System.Collections.Generic;

[Serializable()]
public class Fireball : Battle_Entity_Spells {

    public Fireball() : 
        base(20,
            10,
            1,
            "The user calls upon the power of fire to create a concentrated ball of fire to throw towards their enemies.",
            "Fireball") {}

    public override void CastSpell(List<Battle_Entity> targets, Battle_Entity caster) {
        if (caster.GetStats().currMana < manaCost) {
            return;
        }

        caster.ReduceMana(manaCost);
        targets[0].TakeDamage(caster.GetStats().mag + baseMag, DamageType.Magical);
    }

    public override void PlayAnimation(List<Battle_Entity> targets, List<Battle_Entity> sources) {
        throw new System.NotImplementedException();
    }
}
