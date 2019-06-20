using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PlantKitty.Scripts.Data
{
    // Note: When adding a new consume property, add serialization to ListConsumePropertyJsonConverter.cs

    public class JobChange : ConsumeProperty
    {
        public string job;

        public override string Description
        {
            get
            {
                return $"Allows you to upgrade to {job}!";
            }
        }

        public override void Apply(Character character)
        {
            if (character is Player)
            {
                ((Player)character).SetJobClass(GameData.Instance.GetJob(job));
            }
        }
        public override string ToDataString()
        {
            return $"JobChange$job>{job}";
        }
    }
}
