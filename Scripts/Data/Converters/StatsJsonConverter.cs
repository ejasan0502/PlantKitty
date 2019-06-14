using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantKitty.Scripts.Data.Converters
{
    public class StatsJsonConverter : JsonConverter<Stats>
    {
        public override Stats ReadJson(JsonReader reader, Type objectType, Stats existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            string val = (string)reader.Value;
            return new Stats(val);
        }

        public override void WriteJson(JsonWriter writer, Stats value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }
}
