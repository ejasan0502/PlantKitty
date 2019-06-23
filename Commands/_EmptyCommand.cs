using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantKitty.Commands
{
    // Note: When adding a new command, remember to add to CommandCategory enum in Command script

    public class _EmptyCommand : PlayerCommand
    {
        //[Command("")]
        public async Task InvokeTask()
        {
            await ReplyAsync($"");
        }
    }
}
