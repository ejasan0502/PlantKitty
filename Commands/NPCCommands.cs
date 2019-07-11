using Discord.Commands;
using Discord.WebSocket;
using PlantKitty.Scripts.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantKitty.Commands
{
    // Note: When adding a new command, remember to add to CommandCategory enum in Command script

    public class NPCCommands : PlayerCommand
    {
        [Command("talk")]
        public async Task Talk(string npcName)
        {
            Player player;
            string log;
            if (CheckPlayer(out player, out log))
            {
                Field field = GameData.Instance.GetField(player.field);
                if (field == null) return;

                if (field.HasNPC(npcName))
                {

                }
                else
                    log = $"{Context.User.Mention}. Unable to find {npcName} at current location!";
            }

            if (log != "")
                await ReplyAsync(log);
        }
        [Command("shop")]
        public async Task Shop()
        {
            await ReplyAsync($"");
        }
        [Command("buy")]
        public async Task Buy()
        {
            await ReplyAsync($"");
        }
        [Command("sell")]
        public async Task Sell()
        {
            await ReplyAsync($"");
        }
    }
}
