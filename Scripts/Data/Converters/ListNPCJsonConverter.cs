using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PlantKitty.Scripts.Data.Converters
{
    public class ListNPCJsonConverter : JsonConverter<List<NPC>>
    {
        public override List<NPC> ReadJson(JsonReader reader, Type objectType, List<NPC> existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            List<NPC> values = new List<NPC>();
            string data = (string)reader.Value;
            string[] args = data.Split(',');
            foreach (string arg in args)
            {
                if (arg == "") continue;
                NPC npc = GameData.Instance.GetNPC(arg);
                if (npc == null) continue;
                values.Add(npc);
            }

            return values;
        }

        public override void WriteJson(JsonWriter writer, List<NPC> value, JsonSerializer serializer)
        {
            string data = "";
            foreach (NPC val in value)
            {
                if (data != "") data += ",";
                data += val.name;
            }
            writer.WriteValue(data);
        }
    }
}
