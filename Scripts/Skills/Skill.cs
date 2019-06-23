using Newtonsoft.Json;
using PlantKitty.Scripts.Data;
using PlantKitty.Scripts.Data.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlantKitty.Scripts.Skills
{
    public class Skill
    {
        public string name;
        public bool isFriendly;
        public bool isAoe;
        [JsonConverter(typeof(ListSkillPropertyJsonConverter))] public List<SkillProperty> properties;

        [JsonIgnore]
        public string Description
        {
            get
            {
                string info = "";
                info += "Can only be applied to " + (isFriendly ? "Players" : "Monsters");
                info += isAoe ? "\nArea of Effect" : "\nSingle Target";
                foreach (SkillProperty p in properties)
                    info += "\n" + p.Description();
                return info;
            }
        }

        public void Apply(Character caster, Character target, ref string log)
        {
            foreach (SkillProperty sp in properties)
            {
                if (log != "") log += "\n";
                sp.Apply(caster, target, ref log);
            }
        }
        public void Apply(Character caster, List<Character> targets, ref string log)
        {
            foreach (Character c in targets)
            {
                if (log != "") log += "\n";
                Apply(caster, c, ref log);
            }
        }
    }
}
