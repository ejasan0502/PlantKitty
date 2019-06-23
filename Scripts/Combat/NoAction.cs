using Discord;
using PlantKitty.Scripts.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantKitty.Scripts.Combat
{
    public class NoAction : CombatAction
    {
        public NoAction(Character self)
        {
            this.self = self;
            this.targets = null;
        }

        public override async Task Perform(IMessageChannel channel)
        {
            await channel.SendMessageAsync($"{self.name} does nothing.");
        }
    }
}
