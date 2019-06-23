using Newtonsoft.Json;
using PlantKitty.Scripts.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlantKitty.Scripts.Skills
{
    public abstract class SkillProperty
    {
        public string name;

        public abstract string Description();
        public abstract void Apply(Character caster, Character target, ref string log);
        public abstract string ToDataString();
    }
}
