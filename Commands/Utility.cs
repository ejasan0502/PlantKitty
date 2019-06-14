using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;
using PlantKitty.Scripts.Actions;
using PlantKitty.Scripts.Data;
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
                    log = Context.User.Username + ". " + player.task.Description();
                else
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
                               $"ATK: {player.currentStats.ATK}\n" +
                               $"DEF: {player.currentStats.DEF}\n" +
                               $"ACC: {player.currentStats.ACC}\n" +
                               $"EVA: {player.currentStats.EVA}";

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
                    player.AddAttribute(attribute, amount);
                    await ReplyAsync($"@{Context.User.Username}. {attribute} increased by {amount}");
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
    }
}
