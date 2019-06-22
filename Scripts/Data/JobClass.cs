using Newtonsoft.Json;
using PlantKitty.Scripts.Data.Converters;
using PlantKitty.Scripts.Skills;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlantKitty.Scripts.Data
{
    public struct JobClass
    {
        public string name;
        public string description;
        public Attributes additive;
        [JsonConverter(typeof(ListSkillJsonConverter))] public List<Skill> skills;
    }
}
