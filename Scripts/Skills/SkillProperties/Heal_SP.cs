using System;
using System.Collections.Generic;
using System.Text;
using PlantKitty.Scripts.Data;
using PlantKitty.Scripts.Statuses;

namespace PlantKitty.Scripts.Skills.SkillProperties
{
    public class Heal_SP : SkillProperty
    {
        public bool percent;
        public float amount;

        public override string Description()
        {
            string p = percent ? "%" : "";
            return $"Heals for {amount}{p}.";
        }

        public override void Apply(Character caster, Character target, ref string log)
        {
            float healAmount = amount;
            if (percent) healAmount *= target.maxStats.HP;

            target.Hit(-healAmount);
            log += $"{target.name} has recovered {healAmount}!";
        }

        public override string ToDataString()
        {
            return $"Heal_SP$percent>{percent}$amount>{amount}";
        }
    }
}
