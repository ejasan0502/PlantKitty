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
                                .WithTitle("Crafted...");
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
                    log = "You aren't doing anything.";
            } 
            await ReplyAsync(log);
        }

        [Command("stop")]
        public async Task EndTask()
        {
            Player player;
            string log;
            if (CheckPlayer(out player, out log))
            {
                if (player.task == null)
                {
                    await ReplyAsync($"{Context.User.Username} does not have a task to end...");
                    return;
                }
                if (player.task is Battling)
                {
                    return;
                }

                EmbedBuilder builder = new EmbedBuilder();
                player.EndTask(Context.User.Username, builder);

                await ReplyAsync(null, false, builder.Build());
                PlayerData.Instance.SavePlayer(player.id);
            } else
            {
                await ReplyAsync(log);
            }
        }

        [Command("help")]
        public async Task Help()
        {
            string path = "Resources/Commands.json";
            List<Command> commands = new List<Command>();
            if (File.Exists(path))
            {
                commands = JsonConvert.DeserializeObject<List<Command>>(File.ReadAllText(path));
            }

            EmbedBuilder builder = new EmbedBuilder();
            foreach (Command command in commands)
            {
                builder.AddField(command.name, command.description);
            }

            await ReplyAsync(null, false, builder.Build());
        }

        [Command("stats")]
        public async Task CheckStats()
        {
            Player player;
            string log;
            if (CheckPlayer(out player, out log))
            {
                string stats = $"LVL: {player.level}\n" +
                               $"XP: {player.exp}/{player.GetMaxExp()}\n\n" +
                               $"HP: {player.currentStats.HP}/{player.maxStats.HP}\n" +
                               $"MP: {player.currentStats.MP}/{player.maxStats.MP}\n" +
                               $"PATK: {player.currentStats.PATK}\n" +
                               $"PDEF: {player.currentStats.PDEF}\n" +
                               $"MATK: {player.currentStats.MATK}\n" +
                               $"MDEF: {player.currentStats.MDEF}\n" +
                               $"ACC: {player.currentStats.ACC}\n" +
                               $"EVA: {player.currentStats.EVA}\n" +
                               $"SPD: {player.currentStats.SPD}";

                EmbedBuilder builder = new EmbedBuilder()
                    .WithTitle($"{Context.User.Username}'s Info")
                    .AddField($"Stats", stats);

                await ReplyAsync(null, false, builder.Build());
            } else 
                await ReplyAsync(log);
        }
        [Command("attributes")]
        public async Task Checkattributes(string attribute = "", int amount = 0)
        {
            Player player;
            string log;
            if (CheckPlayer(out player, out log))
            {
                if (attribute != "" && amount > 0)
                {
                    if (player.pointsAvailable < amount) amount = player.pointsAvailable;
                    player.AddAttribute(attribute, amount);
                    await ReplyAsync($"{Context.User.Mention}. {attribute} increased by {amount}");
                    PlayerData.Instance.SavePlayer(player.id);
                } else if (attribute == "")
                {
                    string info = $"STR: {player.attributes.STR}\n" +
                                  $"VIT: {player.attributes.VIT}\n" +
                                  $"DEX: {player.attributes.DEX}\n" +
                                  $"AGI: {player.attributes.AGI}\n" +
                                  $"INT: {player.attributes.INT}\n" +
                                  $"PSY: {player.attributes.PSY}\n" +
                                  $"LUK: {player.attributes.LUK}";

                    EmbedBuilder builder = new EmbedBuilder()
                        .WithTitle($"{Context.User.Username}'s Info")
                        .AddField($"attributes", info)
                        .AddField($"Points Available", player.pointsAvailable);
                    await ReplyAsync(null, false, builder.Build());
                }
            }
            else
                await ReplyAsync(log);
        }
        [Command("equipment")]
        public async Task CheckEquipment()
        {
            Player player;
            string log;
            if (CheckPlayer(out player, out log))
            {
                EmbedBuilder builder = new EmbedBuilder()
                    .WithTitle($"{Context.User.Username}'s equipment");

                foreach (Equip equip in player.equipment)
                {
                    if (equip != null)
                    {
                        builder.AddField(equip.name, equip.Description, true);
                    }
                }

                await ReplyAsync(null, false, builder.Build());
            }
            else
                await ReplyAsync(log);
        }
        [Command("skills")]
        public async Task CheckSkills()
        {
            Player player;
            string log;
            if (CheckPlayer(out player, out log))
            {

                EmbedBuilder builder = new EmbedBuilder()
                    .WithTitle($"{Context.User.Username}'s Skill Library");

                int count = 0;
                foreach (Skill s in player.skills)
                {
                    builder.AddField(s.name, s.Description, count < 4);
                    count++;
                    if (count > 3) count = 0;
                }

                await ReplyAsync(null, false, builder.Build());
            }
            else
                await ReplyAsync(log);
        }
        [Command("learn")]
        public async Task Learn(string skillName = "")
        {
            Player player;
            string log;

            if (CheckPlayer(out player, out log))
            {
                if (skillName == "")
                {
                    // Display learnable skills
                    log = "Skills available to learn:";
                    foreach (Skill s in player.job.skills)
                    {
                        if (log != "") log += "\n";
                        log += s.name;
                    }
                } else if (player.HasSkill(skillName))
                {
                    log = $"{Context.User.Mention}. You have already learned {skillName}";
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
