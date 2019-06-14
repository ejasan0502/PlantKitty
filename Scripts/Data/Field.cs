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
    }
}
