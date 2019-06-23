using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantKitty.Scripts.Data
{
    public struct Command
    {
        public string name;
        public string description;
        public CommandCategory category;
    }

    public enum CommandCategory
    {
        battle,
        craft,
        gather,
        info,
        inventory,
        gesture,
        utility,
        world
    }
}
