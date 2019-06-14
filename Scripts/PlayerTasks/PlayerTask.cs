using Discord;
using PlantKitty.Scripts.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantKitty.Scripts.Actions
{
    public abstract class PlayerTask
    {
        public DateTime startTime;
        public double hoursPassed
        {
            get
            {
                return Math.Round((DateTime.UtcNow - startTime).TotalHours, 2);
            }
        }

        public PlayerTask()
        {
            startTime = DateTime.UtcNow;
        }

        protected void RandomLoot(string username, Player player, EmbedBuilder builder, LootCategory category)
        {
            Dictionary<string, InventoryItem> loot = new Dictionary<string, InventoryItem>();

            Random random = new Random();
            if (hoursPassed >= 1)
            {
                for (double i = 0; i < hoursPassed; i++)
                {
                    Item item = GameData.Instance.GetRandomItem(player.field, category);
                    if (item != null)
                    {
                        if (!loot.ContainsKey(item.name))
                            loot.Add(item.name, new InventoryItem(item, 0));

                        int count = random.Next(1,(int)item.tier);
                        if (count > 0)
                        {
                            loot[item.name].amount += count;
                            player.inventory.AddItem(item, count);
                        }
                    }
                }
            }

            if (builder == null) return;
            builder.WithTitle($"Items added to {username}'s Inventory!")
                .WithCurrentTimestamp();
            foreach (KeyValuePair<string, InventoryItem> l in loot)
            {
                builder.AddField(l.Key + $" x{l.Value.amount}", l.Value.item.Description);
            }
        }

        public abstract string Description();
        public abstract void End(string username, Player player, EmbedBuilder builder);
    }
}
