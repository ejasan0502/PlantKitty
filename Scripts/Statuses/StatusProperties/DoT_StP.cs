using System;
using System.Collections.Generic;
using System.Text;
using PlantKitty.Scripts.Data;

namespace PlantKitty.Scripts.Statuses.StatusProperties
{
    public class DoT_StP : StatusProperty
    {
        public bool percent;
        public DamageType dmgType;
        public float inflict;

        public override string Description(Character caster)
        {
            float atk = dmgType == DamageType.physical ? caster.currentStats.PATK : caster.currentStats.MATK;
            float tick = percent ? atk * inflict : inflict;
            return $"Deal {tick} damage over time.";
        }
        public override void Apply(Character caster, Character target)
        {
            float atk = dmgType == DamageType.physical ? caster.currentStats.PATK : caster.currentStats.MATK;
            target.Hit(percent ? atk * inflict : inflict);
        }
        public override string ToDataString()
        {
            return $"DoT_StP$percent>{percent}$inflict>{inflict}";
        }
    }
}
