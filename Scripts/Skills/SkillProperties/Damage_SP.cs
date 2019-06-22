using System;
using System.Collections.Generic;
using System.Text;
using PlantKitty.Scripts.Data;

namespace PlantKitty.Scripts.Skills.SkillProperties
{
    public class Damage_SP : SkillProperty
    {
        public bool percent;
        public float inflict;

        public override string Description()
        {
            string p = percent ? "%" : "";
            return $"Deal {inflict}{p} damage.";
        }

        public override void Apply(Character caster, Character target)
        {
            float rawDmg = caster.currentStats.ATK;
            if (percent)
                rawDmg *= inflict;
            else
                rawDmg += inflict;

            float dmg = rawDmg - target.currentStats.DEF;
            if (dmg < 1f) dmg = 1f;

            target.Hit(dmg);
        }

        public override string ToDataString()
        {
            return $"Damage_SP$percent>{percent}$inflict>{inflict}";
        }
    }
}
