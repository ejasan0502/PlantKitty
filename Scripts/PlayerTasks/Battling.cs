using Discord;
using PlantKitty.Scripts.Combat;
using PlantKitty.Scripts.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantKitty.Scripts.Actions
{
    public class Battling : PlayerTask
    {
        public string battleId;

        public Battling(string battleId) : base(){
            this.battleId = battleId;
        }

        public override string Description()
        {
            return $"You are in combat!";
        }
        public override void End(string username, Player player, EmbedBuilder builder)
        {
            
        }
    }
}
