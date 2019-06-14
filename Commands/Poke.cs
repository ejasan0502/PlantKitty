using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantKitty.Commands
{
    public class Poke : ModuleBase<SocketCommandContext>
    {
        [Command("poke")]
        public async Task InvokeAsync(SocketGuildUser user)
        {
            await ReplyAsync($"{ Context.User.Username } pokes { user.Mention }!");
        }
    }
}
