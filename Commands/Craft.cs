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
        public async Task CraftItem()
        {
            string log = "Item types that are craftable...";
            string[] itemTypes = Enum.GetNames(typeof(ItemType));
            foreach (string s in itemTypes)
                log += "\n" + s;

            await ReplyAsync(log);
        }
        
        [Command]
        public async Task CraftItem(string itemType)
        {
            Player player;
            string log;

            if (CheckPlayer(out player, out log))
            {
                List<string> itemTypes = Enum.GetNames(typeof(ItemType)).ToList();
                if (itemTypes.Contains(itemType))
                {
                    List<Recipe> recipes = GameData.Instance.GetRecipes(itemType);
                    if (recipes == null) return;
                    recipes.Sort((x, y) => string.Compare(x.product, y.product));

                    EmbedBuilder builder = new EmbedBuilder()
                        .WithTitle($"Displaying first 10 recipes of {itemType}");

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
            }
            else
                await ReplyAsync(log);
        }

        [Command]
        public async Task CraftItem(string itemName, int amount)
        {
            Player player;
            string log;

            if (CheckPlayer(out player, out log))
            {
                Recipe recipe = GameData.Instance.GetRecipe(itemName);
                if (recipe == null) return;

                if (recipe.CanCraft(player.inventory, amount))
                {
                    Crafting crafting;
                    if (player.task == null)
                    {
                        crafting = new Crafting();
                        player.SetTask(crafting);
                    } else if (player.task is Crafting)
                        crafting = player.task as Crafting;
                    else
                    {
                        await ReplyAsync($"{Context.User.Username} is busy!");
                        return;
                    }

                    crafting.AddCraft(recipe, amount, player.inventory);
                    await ReplyAsync($"{Context.User.Mention}. Added {amount} {itemName} to crafting queue!");
                    PlayerData.Instance.SavePlayer(player.id);
                }
                else
                    await ReplyAsync($"{Context.User.Mention}. Not enough materials!");
            }
            else
                await ReplyAsync(log);
        }

        [Command("check")]
        public async Task CheckCraft(string itemName)
        {
            Recipe recipe = GameData.Instance.GetRecipe(itemName);
            if (recipe != null)
            {
                await ReplyAsync($"{itemName} Recipe:\n{recipe.Ingredients}");
            } else
            {
                await ReplyAsync($"There is no recipe for {itemName}...");
            }
        }
    }
}
