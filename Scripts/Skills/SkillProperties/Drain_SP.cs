using System;
using System.Collections.Generic;
using System.Text;
using PlantKitty.Scripts.Data;
using PlantKitty.Scripts.Statuses;

namespace PlantKitty.Scripts.Skills.SkillProperties
{
    public class Drain_SP : SkillProperty
    {
        public bool percent;
        public float amount;

        public override string Description()
        {
            string p = percent ? "%" : "";
            return $"Drains mana for {amount}{p}.";
        }

        public override void Apply(Character caster, Character target, ref string log)
        {
            float manaAmount = amount;
            if (percent) manaAmount *= target.maxStats.MP;

            target.ConsumeMP(manaAmount);
            log += $"{target.name} has been drained of {manaAmount} mana!";
        }

        public override string ToDataString()
        {
            return $"Drain_SP$percent>{percent}$amount>{amount}";
        }
    }
}
