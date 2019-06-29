using System;
using System.Collections.Generic;
using System.Text;
using PlantKitty.Scripts.Data;
using PlantKitty.Scripts.Statuses;

namespace PlantKitty.Scripts.Skills.SkillProperties
{
    public class Cure_SP : SkillProperty
    {
        public List<string> statuses;

        public override string Description()
        {
            string data = "";
            foreach (string val in statuses)
            {
                if (data != "") data += ", ";
                data += val;
            }
            return data;
        }

        public override void Apply(Character caster, Character target, ref string log)
        {
            target.RemoveStatus(statuses);
            log += $"{target.name} has been cured!";
        }

        public override string ToDataString()
        {
            string data = "";
            foreach (string val in statuses)
            {
                if (data != "") data += ",";
                data += val;
            }
            return $"Heal_SP$statuses>{data}";
        }
    }
}
