using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantKitty.Scripts.Data.Converters
{
    public class AttributesJsonConverter : JsonConverter<Attributes>
    {
        public override Attributes ReadJson(JsonReader reader, Type objectType, Attributes existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            string val = (string)reader.Value;
            return new Attributes(val);
        }

        public override void WriteJson(JsonWriter writer, Attributes value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }
}
