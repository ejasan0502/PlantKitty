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
            foreach (Character target in targets)
            {
                if (random.Next(0, 100) <= self.currentStats.ACC - target.currentStats.EVA)
                {
                    skill.Apply(self, target);
                    await channel.SendMessageAsync($"{self.name} casted {skill.name}" + (skill.isAoe ? "!" : $" on {target.name}!"));
                }
                else
                    await channel.SendMessageAsync($"{self.name} missed {target.name}...");
            }
        }
    }
}
