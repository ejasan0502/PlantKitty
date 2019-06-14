using Discord.Commands;
using PlantKitty.Scripts.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantKitty.Commands
{
    public class PlayerCommand : ModuleBase<SocketCommandContext>
    {
        protected bool CheckIfBusy(out Player player, out string errors)
        {
            if (!CheckPlayer(out player, out errors))
                return false;
            if (player.task != null)
            {
                errors = $"{Context.User.Username} is busy!";
                return false;
            }
            errors = "";
            return true;
        }
        protected bool CheckPlayer(out Player player, out string error)
        {
            player = PlayerData.Instance.GetPlayer(Context.User.Id);
            if (player == null)
            {
                error = $"{ Context.User.Username} is not registered!";
                return false;
            }
            error = "";
            return true;
        }
    }
}
