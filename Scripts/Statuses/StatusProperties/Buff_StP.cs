using System;
using System.Collections.Generic;
using System.Text;
using PlantKitty.Scripts.Data;

namespace PlantKitty.Scripts.Statuses.StatusProperties
{
    public class Buff_StP : StatusProperty
    {
        public bool percent;
        public Stats stats;

        public override string Description(Character caster)
        {
            return stats.ToStringWithSigns(percent);
        }
        public override void Apply(Character caster, Character target)
        {
            if (percent)
                target.maxStats *= stats;
            else
                target.maxStats += stats;
        }
        public override string ToDataString()
        {
            return $"Buff_StP$percent>{percent}$stats>{stats.ToString()}";
        }
    }
}
