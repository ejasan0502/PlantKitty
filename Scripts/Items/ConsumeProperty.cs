using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantKitty.Scripts.Data
{
    public abstract class ConsumeProperty
    {
        [JsonIgnore]
        public abstract string Description { get; }
        public abstract void Apply(Character character);
        public abstract string ToDataString();
        public abstract string OnConsume(Character target);
    }
}
