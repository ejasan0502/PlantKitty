using Discord;
using PlantKitty.Scripts.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantKitty.Scripts.Combat
{
    public class Attack : CombatAction
    {
        public Attack(Character self, Character target)
        {
            this.self = self;
            this.targets = new List<Character>() { target };
        }

        public override async Task Perform(IMessageChannel channel)
        {
            if (self.currentStats.HP <= 0) return;

            Random random = new Random();
            foreach (Character target in targets)
            {
                if (random.Next(0, 100) <= self.currentStats.ACC - target.currentStats.EVA)
                {
                    bool isCrit = random.Next(0, 100) <= self.currentStats.CRIT;

                    float inflict = self.currentStats.PATK - target.currentStats.PDEF;
                    if (inflict < 1) inflict = 1f;

                    target.Hit(inflict);
                    await channel.SendMessageAsync($"{target.name} takes {inflict} damage!");
                }
                else
                    await channel.SendMessageAsync($"{self.name} missed {target.name}...");
            }
        }
    }
}
