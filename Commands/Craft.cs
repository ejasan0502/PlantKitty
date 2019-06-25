using Discord;
using Discord.Commands;
using Discord.WebSocket;
using PlantKitty.Scripts.Actions;
using PlantKitty.Scripts.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantKitty.Commands
{
    [Group("craft")]
    public class Craft : PlayerCommand
    {
        [Command]
        public async Task DisplayItemTypes()
        {
            string log = $"{Context.User.Mention}. Item types that are craftable are:";
            string[] itemTypes = Enum.GetNames(typeof(ItemType));
            foreach (string s in itemTypes)
                log += "\n" + s;
            await ReplyAsync(log);
        }

        [Command]
        public async Task DisplayRecipes(string itemType)
        {
            List<string> itemTypes = Enum.GetNames(typeof(ItemType)).ToList();
            if (itemTypes.Contains(itemType))
            {
                List<Recipe> recipes = GameData.Instance.GetRecipes(itemType);
                if (recipes == null) return;
                recipes.Sort((x, y) => string.Compare(x.product, y.product));

                EmbedBuilder builder = new EmbedBuilder()
                    .WithTitle($"{Context.User.Mention}. Displaying recipes of {itemType}:");

                int count = 0;
                for (int i = 0; i < recipes.Count; i++)
                {
                    if (i >= 10) break;

                    builder.AddField(recipes[i].product, recipes[i].Ingredients, count < 4);
                    count++;
                    if (count > 3) count = 0;
                }

                await ReplyAsync(null, false, builder.Build());
            }
            else
                await ReplyAsync($"{Context.User.Mention}. Unknown item type...");
        }

        [Command]
        public async Task CraftItem(string itemName, int amount)
        {
            Player player;
            string log;

            if (CheckPlayer(out player, out log))
            {
                if ((player.task != null && player.task is Crafting) || player.task == null)
                {
                    Recipe recipe = GameData.Instance.GetRecipe(itemName);
                    if (recipe != null)
                    {
                        if (recipe.CanCraft(player.inventory, amount))
                        {
                            if (player.task == null) player.SetTask(new Crafting());
                            Crafting crafting = player.task as Crafting;

                            crafting.AddCraft(recipe, amount, player.inventory);
                            await ReplyAsync($"{Context.User.Mention}. Added {amount} {itemName} to crafting queue!");
                            PlayerData.Instance.SavePlayer(player.id);
                        }
                        else
                            log = $"{Context.User.Mention}. Not enough materials!";
                    }
                    else
                        log = $"{Context.User.Mention}. Unknown recipe...";
                }
                else
                    log = $"{Context.User.Mention}. You are busy!";
            }

            if (log != "")
                await ReplyAsync(log);
        }

        [Command("check")]
        public async Task CheckCraft(string itemName)
        {
            string log;

            Recipe recipe = GameData.Instance.GetRecipe(itemName);
            if (recipe != null)
            {
                log = $"{itemName} Recipe:\n{recipe.Ingredients}";
            } else
            {
                log = $"There is no recipe for {itemName}...";
            }

            await ReplyAsync(log);
        }
    }
}
