using Discord;
using PlantKitty.Scripts.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantKitty.Scripts.Combat
{
    public class Use : CombatAction
    {
        private Consumable consumable;
        private Inventory inventory;

        public Use(Consumable consumable, Player self, Character target)
        {
            this.consumable = consumable;
            this.self = self;
            this.inventory = self.inventory;
            this.targets = new List<Character>() { target };
        }

        public override async Task Perform(IMessageChannel channel)
        {
            if (self.currentStats.HP <= 0) return;

            string log = $"{self.name} uses {consumable.name}!";
            foreach (Character target in targets)
            {
                consumable.Use(target, ref log);
            }
            inventory.RemoveItem(consumable, 1);
            await channel.SendMessageAsync(log);
        }
    }
}
