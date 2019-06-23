using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;
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
    public class InfoCommands : PlayerCommand
    {
        [Command("help")]
        public async Task Help()
        {
            string[] categories = Enum.GetNames(typeof(CommandCategory));
            string log = $"{Context.User.Mention}. All command categories are:";
            foreach (string s in categories)
            {
                log += "\n" + s;
            }

            await ReplyAsync(log);
        }
        [Command("help")]
        public async Task Help(CommandCategory category)
        {
            string path = "Resources/Commands.json";
            List<Command> commands = new List<Command>();
            if (File.Exists(path))
            {
                commands = JsonConvert.DeserializeObject<List<Command>>(File.ReadAllText(path));
            }

            string log = "";
            foreach (Command command in commands)
            {
                if (command.category == category)
                {
                    if (log != "") log += "\n";
                    log += $"{command.name}. {command.description}";
                }
            }

            await ReplyAsync(log);
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
                               $"SPD: {player.currentStats.SPD}\n" +
                               $"CRIT: {player.currentStats.CRIT}\n" +
                               $"CRITDMG: {player.currentStats.CRITDMG}";

                EmbedBuilder builder = new EmbedBuilder()
                    .WithTitle($"{Context.User.Username}'s Info")
                    .AddField($"Stats", stats);

                await ReplyAsync(null, false, builder.Build());
            }
            else
                await ReplyAsync(log);
        }

        [Command("attributes")]
        public async Task DisplayAttributes()
        {
            Player player;
            string log;
            if (CheckPlayer(out player, out log))
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
            else
                await ReplyAsync(log);
        }
        [Command("attributes")]
        public async Task AddAttributes(string attribute, int amount = 1)
        {
            Player player;
            string log;
            if (CheckPlayer(out player, out log))
            {
                if (IsInBattle(player))
                {
                    log = $"{Context.User.Mention}. This command is not available during combat!";
                }
                else
                {
                    if (player.pointsAvailable < amount) amount = player.pointsAvailable;
                    player.AddAttribute(attribute, amount);
                    await ReplyAsync($"{Context.User.Mention}. {attribute} increased by {amount}");
                    PlayerData.Instance.SavePlayer(player.id);
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
    }
}
