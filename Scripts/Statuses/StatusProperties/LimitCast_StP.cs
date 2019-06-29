using System;
using System.Collections.Generic;
using System.Text;
using PlantKitty.Scripts.Data;

namespace PlantKitty.Scripts.Statuses.StatusProperties
{
    public class LimitCast_StP : StatusProperty
    {
        public override string Description(Character caster)
        {
            return "Prevents target from casting.";
        }
        public override void Apply(Character caster, Character target)
        {
            target.SetCanCast(false);
        }
        public override string ToDataString()
        {
            return $"LimitCast_StP";
        }
    }
}
