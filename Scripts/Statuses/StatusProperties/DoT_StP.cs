using System;
using System.Collections.Generic;
using System.Text;
using PlantKitty.Scripts.Data;

namespace PlantKitty.Scripts.Statuses.StatusProperties
{
    public class DoT_StP : StatusProperty
    {
        public bool percent;
        public float inflict;

        public override string Description(Character caster)
        {
            float tick = percent ? caster.currentStats.ATK * inflict : inflict;
            return $"Deal {tick} damage over time.";
        }
        public override void Apply(Character caster, Character target)
        {
            target.Hit(percent ? caster.currentStats.ATK * inflict : inflict);
        }
        public override string ToDataString()
        {
            return $"DoT_StP$percent>{percent}$inflict>{inflict}";
        }
    }
}
