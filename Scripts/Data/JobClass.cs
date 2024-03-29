﻿using Newtonsoft.Json;
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
        public Attributes attributes;
        public Stats stats;
        [JsonConverter(typeof(ListStringJsonConverter))] public List<string> skills;
    }
}
