using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantKitty.Scripts.Data
{
    public class Replenish : ConsumeProperty
    {
        public float amount;

        public override string Description
        {
            get
            {
                return $"Replenish {amount} MP";
            }
        }

        public override void Apply(Character character)
        {
            character.Replenish(amount);
        }

        public override string OnConsume(Character target)
        {
            return $"{target.name} has recovered {amount} MP!";
        }

        public override string ToDataString()
        {
            return $"Replenish$amount>{amount}";
        }
    }
}
