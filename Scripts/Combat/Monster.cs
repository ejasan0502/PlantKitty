using Newtonsoft.Json;
using PlantKitty.Scripts.Data;
using PlantKitty.Scripts.Data.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PlantKitty.Scripts.Combat
{
    public class Monster : Character
    {
        public float exp;
        [JsonConverter(typeof(ListStringJsonConverter))] public List<string> loot;

        public Monster()
        {

        }
        public Monster(Monster m)
        {
            name = m.name;
            currentStats = new Stats(m.currentStats.ToString());
            maxStats = new Stats(m.maxStats.ToString());
            exp = m.exp;
            loot = new List<string>(m.loot);
        }
    }
}
