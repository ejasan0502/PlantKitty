using Newtonsoft.Json;
using PlantKitty.Scripts.Data.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantKitty.Scripts.Data
{
    public class Field
    {
        public string name;
        [JsonConverter(typeof(ListStringJsonConverter))] public List<string> loot;
        [JsonConverter(typeof(ListStringJsonConverter))] public List<string> monsters;
        [JsonConverter(typeof(ListNPCJsonConverter))] public List<NPC> npcs;

        public bool HasNPC(string npcName)
        {
            foreach (NPC npc in npcs)
                if (npc.name == npcName)
                    return true;
            return false;
        }
    }
}
