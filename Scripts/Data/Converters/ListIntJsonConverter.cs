using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantKitty.Scripts.Data.Converters
{
    public class ListIntJsonConverter : JsonConverter<List<int>>
    {
        public override List<int> ReadJson(JsonReader reader, Type objectType, List<int> existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            List<int> values = new List<int>();
            string data = (string)reader.Value;
            string[] args = data.Split(',');
            foreach (string arg in args)
                values.Add(int.Parse(arg));

            return values;
        }

        public override void WriteJson(JsonWriter writer, List<int> value, JsonSerializer serializer)
        {
            string data = "";
            foreach (int val in value)
            {
                if (data != "") data += ",";
                data += val;
            }
            writer.WriteValue(data);
        }
    }
}
