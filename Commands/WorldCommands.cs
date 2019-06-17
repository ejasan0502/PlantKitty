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
    public class WorldCommands : PlayerCommand
    {
        private void CheckTraveling(string username, Player player)
        {
            if (player.task != null)
            {
                if (player.task is Traveling)
                {
                    Traveling travelTask = player.task as Traveling;
                    if (travelTask.hoursPassed >= 1)
                    {
                        player.EndTask(username, null);
                    }
                }
            }
        }

        [Command("travel")]
        public async Task Travel(string fieldName = "")
        {
            Player player;
            string log;
            if (CheckIfBusy(out player, out log))
            {
                if (fieldName != "")
                {
                    CheckTraveling(Context.User.Username, player);
                    if (player.field == fieldName)
                    {
                        await ReplyAsync($"{Context.User.Username} is already in {fieldName}!");
                        return;
                    }

                    player.SetTask(new Traveling(fieldName));
                    await ReplyAsync($"{Context.User.Username} travels to {fieldName}! ETA: 1 HR");
                    PlayerData.Instance.SavePlayer(player.id);
                } else
                {
                    EmbedBuilder builder = new EmbedBuilder()
                        .WithTitle("Locations");

                    foreach (Field field in GameData.Instance.GetFields())
                    {
                        string loot = "";
                        for (int i = 0; i < field.loot.Count; i++)
                        {
                            if (i != 0) loot += "\n";
                            loot += field.loot[i];
                        }

                        builder.AddField(field.name, loot, true);
                    }

                    await ReplyAsync(null, false, builder.Build());
                }
            } else
            {
                await ReplyAsync(log);
            }
        }

        [Command("location")]
        public async Task Location()
        {
            Player player;
            string log;
            if (CheckPlayer(out player, out log))
            {
                CheckTraveling(Context.User.Username, player);

                Field field = GameData.Instance.GetField(player.field);
                if (field == null) return;
                string loot = "";
                foreach (string s in field.loot)
                    loot += s + "\n";
                loot.Substring(loot.Length - 2);

                EmbedBuilder builder = new EmbedBuilder()
                    .WithTitle($"{Context.User.Username}\nYou are currently at {player.field}")
                    .AddField("Loot", loot);
                await ReplyAsync(null, false, builder.Build());
            } else
                await ReplyAsync(log);
        }
    }
}
