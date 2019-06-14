using Discord;
using PlantKitty.Scripts.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantKitty.Scripts.Combat
{
    public abstract class CombatAction
    {
        protected Character self;
        protected List<Character> targets;

        public Character GetSelf()
        {
            return self;
        }

        public virtual async Task Perform(IMessageChannel channel)
        {
            await Task.Delay(1);
        }
    }
}
