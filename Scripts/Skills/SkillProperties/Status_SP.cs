using System;
using System.Collections.Generic;
using System.Text;
using PlantKitty.Scripts.Data;
using PlantKitty.Scripts.Statuses;

namespace PlantKitty.Scripts.Skills.SkillProperties
{
    public class Status_SP : SkillProperty
    {
        public float chance;
        public string status;

        public override string Description()
        {
            Status st = GameData.Instance.GetStatus(status);
            return $"{chance}% chance to inflict {status}.\n" + (st.name != "" ? st.Description() : "");
        }

        public override void Apply(Character caster, Character target)
        {
            Random random = new Random();
            if (random.Next(0, 100) > chance) return;

            target.AddStatus(caster, status);
        }

        public override string ToDataString()
        {
            return $"Status_SP$chance>{chance}$status>{status}";
        }
    }
}
