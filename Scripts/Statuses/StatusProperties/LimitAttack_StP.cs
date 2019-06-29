using System;
using System.Collections.Generic;
using System.Text;
using PlantKitty.Scripts.Data;

namespace PlantKitty.Scripts.Statuses.StatusProperties
{
    public class LimitAttack_StP : StatusProperty
    {
        public override string Description(Character caster)
        {
            return "Prevents target from attacking.";
        }
        public override void Apply(Character caster, Character target)
        {
            target.SetCanAttack(false);
        }
        public override string ToDataString()
        {
            return $"LimitAttack_StP";
        }
    }
}
