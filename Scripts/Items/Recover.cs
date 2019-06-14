using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PlantKitty.Scripts.Data
{
    public class Recover : ConsumeProperty
    {
        public float amount;

        public override string Description
        {
            get
            {
                return $"Recover {amount} HP";
            }
        }

        public override void Apply(Character character)
        {
            character.Hit(-amount);
        }
        public override string ToDataString()
        {
            return $"Recover$amount>{amount}";
        }
    }
}
