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
            await ReplyAsync($"");
        }
    }
}
