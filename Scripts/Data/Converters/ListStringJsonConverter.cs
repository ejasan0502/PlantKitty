using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantKitty.Scripts.Data.Converters
{
    public class ListStringJsonConverter : JsonConverter<List<string>>
    {
        public override List<string> ReadJson(JsonReader reader, Type objectType, List<string> existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            List<string> values = new List<string>();
            string data = (string)reader.Value;
            string[] args = data.Split(',');
            foreach (string arg in args)
                values.Add(arg);

            return values;
        }

        public override void WriteJson(JsonWriter writer, List<string> value, JsonSerializer serializer)
        {
            string data = "";
            foreach (string val in value)
            {
                if (data != "") data += ",";
                data += val;
            }
            writer.WriteValue(data);
        }
    }
}
