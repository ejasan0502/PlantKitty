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
    [Group("admin")]
    public class Admin : PlayerCommand
    {
#if DEBUG
        [Command("get")]
        public async Task Give(string itemName, int amount)
        {
            Player player;
            string log;
            if (CheckPlayer(out player, out log))
            {
                Item item = GameData.Instance.GetItem(itemName);
                if (item == null) return;

                player.inventory.AddItem(item, amount);
                await ReplyAsync($"You gained x{amount} {item.name}");
                PlayerData.Instance.SavePlayer(player.id);
            } else 
                await ReplyAsync(log);
        }
#endif
    }
}
