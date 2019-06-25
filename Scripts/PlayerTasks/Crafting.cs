using Discord;
using Newtonsoft.Json;
using PlantKitty.Scripts.Combat;
using PlantKitty.Scripts.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantKitty.Scripts.Actions
{
    public class Crafting : PlayerTask
    {
        private const int maxQueue = 5;

        public DateTime start;
        public List<CraftItem> craftQueue;

        public Crafting() : base(){
            start = DateTime.UtcNow;
            craftQueue = new List<CraftItem>();
        }

        public override string Description()
        {
            string description = "";
            if (craftQueue.Count > 0)
            {
                for (int i = 0; i < craftQueue.Count; i++)
                {
                    description += $"\n{craftQueue[i].itemName} x{craftQueue[i].amount}";
                }
            } else
            {
                description += "Empty";
            }
            return description;
        }
        public override void End(string username, Player player, EmbedBuilder builder)
        {
            builder.WithTitle($"Added to {username}'s inventory.");

            List<InventoryItem> items = Check(player, player.inventory);
            foreach (InventoryItem slot in items)
            {
                builder.AddField(slot.item.name + " x" + slot.amount, slot.item.Description, true);
            }

            player.SetTask(null);
        }

        public void AddCraft(Recipe recipe, int amount, Inventory inventory)
        {
            if (craftQueue.Count < maxQueue && recipe.CanCraft(inventory))
            {
                CraftItem craft = new CraftItem()
                {
                    itemName = recipe.product,
                    amount = amount,
                    craftTime = recipe.time
                };

                craftQueue.Add(craft);

                // Remove materials from inventory
                for (int i = 0; i < recipe.materials.Count; i++)
                {
                    inventory.RemoveItem(recipe.materials[i], (i < recipe.amounts.Count ? recipe.amounts[i] : 1)*amount);
                }
            }
        }
        public List<InventoryItem> Check(Player player, Inventory inventory)
        {
            int remainder = -1;
            List<InventoryItem> gained = new List<InventoryItem>();

            // Find total amount of minutes passed
            double timeAccumulated = (DateTime.UtcNow - start).TotalMinutes;

            // Add items based on time
            for (int i = 0; i < craftQueue.Count; i++)
            {
                int amount = craftQueue[i].amount;
                double timeNeeded = craftQueue[i].craftTime * amount;
                if (timeNeeded > timeAccumulated)
                {
                    remainder = i;
                    amount = (int)Math.Floor(timeAccumulated / craftQueue[i].craftTime);
                    timeNeeded = craftQueue[i].craftTime * amount;
                }

                if (amount > 0)
                {
                    craftQueue[i].amount -= amount;

                    Item item = GameData.Instance.GetItem(craftQueue[i].itemName);
                    timeAccumulated -= timeNeeded;
                    inventory.AddItem(item, amount);
                    gained.Add(new InventoryItem(item, amount));

                    if (remainder != -1)
                    {
                        // Reset time
                        if (timeAccumulated > 0)
                        {
                            start = DateTime.UtcNow;
                            start.AddMinutes(-timeAccumulated);
                        }

                        // Clear crafting queue
                        craftQueue.RemoveRange(0, remainder - 1);
                        if (craftQueue.Count > 0 && craftQueue[0].amount < 1)
                        {
                            craftQueue.RemoveAt(0);
                        }

                        break;
                    }
                }
            }

            return gained;
        }
    }

    public class CraftItem
    {
        public string itemName;
        public int amount;
        public double craftTime;
    }
}
