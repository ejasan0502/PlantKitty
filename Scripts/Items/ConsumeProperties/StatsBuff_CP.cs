using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PlantKitty.Scripts.Data
{
    public class StatsBuff_CP : ConsumeProperty
    {
        public StatsBuff buff;

        public override string Description
        {
            get
            {
                return buff.Description;
            }
        }

        public StatsBuff_CP(string[] args)
        {
            buff = new StatsBuff()
            {
                percent = bool.Parse( args[1].Split('>')[1]),
                stats = new Stats(args[2].Split('>')[1])
            };
        }

        public override void Apply(Character character)
        {
            if (character is Player)
            {
                Player p = character as Player;
                p.AddBuff(buff);
            }
        }
        public override string ToDataString()
        {
            return $"StatsBuff_CP$percent>{buff.percent}$stats>{buff.stats.ToString()}";
        }

        public override string OnConsume(Character target)
        {
            return $"{target.name} has been buffed!";
        }
    }
}
