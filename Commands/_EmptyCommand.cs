using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantKitty.Commands
{
    public class _EmptyCommand : PlayerCommand
    {
        //[Command("")]
        public async Task InvokeTask()
        {
            await ReplyAsync($"");
        }
    }
}
