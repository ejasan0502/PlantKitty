using Discord;
using PlantKitty.Scripts.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantKitty.Scripts.Actions
{
    public class Fishing : PlayerTask
    {
        public Fishing() : base(){
            
        }

        public override string Description()
        {
            return $"You are currently fishing... {hoursPassed} hrs passed.";
        }
        public override void End(string username, Player player, EmbedBuilder builder)
        {
            RandomLoot(username, player, builder, LootCategory.fishing);
        }
    }
}
