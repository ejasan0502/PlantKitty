using Discord;
using PlantKitty.Scripts.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantKitty.Scripts.Actions
{
    public class Traveling : PlayerTask
    {
        public string fieldName; 

        public Traveling(string fieldName) : base(){
            this.fieldName = fieldName;
        }

        public override string Description()
        {
            return $"You are currently traveling... {hoursPassed} hrs passed.";
        }
        public override void End(string username, Player player, EmbedBuilder builder)
        {
            if (hoursPassed >= 1)
                player.field = fieldName;
        }
    }
}
