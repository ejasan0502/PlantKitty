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

        public Command(string name, string description)
        {
            this.name = name;
            this.description = description;
        }
    }
}
