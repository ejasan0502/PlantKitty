using Discord;
using PlantKitty.Scripts.Data;
using PlantKitty.Scripts.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantKitty.Scripts.Combat
{
    public class Cast : CombatAction
    {
        public Skill skill;

        public Cast(Character self, Character target, Skill skill)
        {
            this.self = self;
            this.targets = new List<Character>() { target };
            this.skill = skill;
        }
        public Cast(Character self, List<Character> targets, Skill skill)
        {
            this.self = self;
            this.targets = targets;
            this.skill = skill;
        }

        public override async Task Perform(IMessageChannel channel)
        {
            if (self.currentStats.HP <= 0) return;

            Random random = new Random();
            if (skill.isAoe)
            {
                await channel.SendMessageAsync($"{self.name} casts {skill.name}!");
            }

            foreach (Character target in targets)
            {
                if (random.Next(0, 100) <= self.currentStats.ACC - target.currentStats.EVA)
                {
                    string log = !skill.isAoe ? $"{self.name} casts {skill.name} on {target.name}!" : "";
                    skill.Apply(self, target, ref log);
                    await channel.SendMessageAsync(log);
                }
                else
                    await channel.SendMessageAsync($"{self.name} missed {target.name}...");
            }
        }
    }
}
