using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;
using PlantKitty.Scripts.Actions;
using PlantKitty.Scripts.Data;
using PlantKitty.Scripts.Skills;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantKitty.Commands
{
    public class Utility : PlayerCommand
    {
        [Command("status")]
        public async Task CheckStatus()
        {
            Player player;
            string log;
            if (CheckPlayer(out player, out log))
            {
                if (player.task != null)
                {
                    if (player.task is Crafting)
                    {
                        Crafting crafting = player.task as Crafting;

                        List<InventoryItem> craftedItems = crafting.Check(player, player.inventory);
                        if (craftedItems.Count > 0)
                        {
                            EmbedBuilder builder = new EmbedBuilder()
                                .WithTitle($"{Context.User.Mention}. Crafted...");
                            foreach (InventoryItem slot in craftedItems)
                            {
                                builder.AddField(slot.item.name + " x" + slot.amount, slot.item.Description, true);
                            }
                            await ReplyAsync(null, false, builder.Build());
                            PlayerData.Instance.SavePlayer(player.id);
                        }

                        // Remove task if no more recipes in crafting queue
                        if (crafting.craftQueue.Count < 1)
                            player.SetTask(null);

                        log = Context.User.Mention + ". Your crafting queue:" + player.task.Description();
                    }
                    else
                        log = Context.User.Mention + "." + player.task.Description();
                } else
                    log = $"{Context.User.Mention}. You aren't doing anything.";
            } 

            if (log != "")
                await ReplyAsync(log);
        }

        [Command("stop")]
        public async Task EndTask()
        {
            Player player;
            string log;
            if (CheckPlayer(out player, out log))
            {
                if (player.task != null && !(player.task is Battling))
                {
                    EmbedBuilder builder = new EmbedBuilder();
                    player.EndTask(Context.User.Username, builder);
                    await ReplyAsync(null, false, builder.Build());

                    PlayerData.Instance.SavePlayer(player.id);
                } else
                {
                    if (player.task == null) log = $"{Context.User.Mention}. You do not have a task to end...";
                    else if (player.task is Battling) log = $"{Context.User.Mention}. You must finish the battle!";
                }
            }

            if (log != "")
                await ReplyAsync(log);
        }

        [Command("learn")]
        public async Task DisplayLearnableSkills()
        {
            Player player;
            string log;

            if (CheckPlayer(out player, out log))
            {
                // Display learnable skills
                log = $"{Context.User.Mention}. Skills available to learn:";
                foreach (string s in player.job.skills)
                {
                    if (log != "") log += "\n";
                    log += s;
                }
            }

            if (log != "")
                await ReplyAsync(log);
        }

        [Command("learn")]
        public async Task Learn(string skillName)
        {
            Player player;
            string log;

            if (CheckPlayer(out player, out log))
            {
                if (player.HasSkill(skillName))
                {
                    log = $"{Context.User.Mention}. You have already learned {skillName}!";
                } else
                {
                    Skill skill = GameData.Instance.GetSkill(skillName);
                    if (skill != null)
                    {
                        player.AddSkill(skill);
                        log = $"{Context.User.Mention}. You have learned {skillName}!";
                    } else
                    {
                        log = $"{Context.User.Mention}. Unknown skill name, {skillName}...";
                    }
                }
            }

            if (log != "")
                await ReplyAsync(log);
        }

        [Command("save")]
        public async Task SavePlayer()
        {
            PlayerData.Instance.SavePlayer(Context.User.Id);
            await ReplyAsync($"{Context.User.Mention}. Data saved!");
        }
        [Command("bug")]
        public async Task BugReport(params string[] vals)
        {
            string report = string.Join(" ", vals);
            File.AppendAllText("Resources/Bugs.txt", " - " + report + Environment.NewLine);

            await ReplyAsync($"{Context.User.Mention}. Bug report sent!");
        }
        [Command("reset")]
        public async Task Reset()
        {
            PlayerData.Instance.NewPlayer(Context.User.Id, Context.User.Username);
            await ReplyAsync($"{Context.User.Username} has started over!");
        }

    }
}