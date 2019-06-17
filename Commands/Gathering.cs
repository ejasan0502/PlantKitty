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
    public class Gathering : PlayerCommand
    {
        private PlayerTask GetTask(LootCategory category)
        {
            switch (category)
            {
                case LootCategory.fishing: return new Fishing();
                case LootCategory.foraging: return new Foraging();
                case LootCategory.hunting: return new Hunting();
                case LootCategory.mining: return new Mining();
            }
            return null;
        }
        private async Task Gather(LootCategory category)
        {
            Player player;
            string log;
            if (CheckIfBusy(out player, out log))
            {
                PlayerTask task = GetTask(category);
                player.SetTask(task);

                log = $"{Context.User.Mention} {task.Description()}";
                PlayerData.Instance.SavePlayer(player.id);
            }

            await ReplyAsync(log);
        }

        [Command("foraging")]
        public async Task Foraging()
        {
            await Gather(LootCategory.foraging);
        }
        [Command("fishing")]
        public async Task Fishing()
        {
            await Gather(LootCategory.fishing);
        }
        [Command("hunting")]
        public async Task Hunting()
        {
            await Gather(LootCategory.hunting);
        }
        [Command("mining")]
        public async Task Mining()
        {
            await Gather(LootCategory.mining);
        }
    }
}
