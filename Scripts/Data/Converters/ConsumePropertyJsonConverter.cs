using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantKitty.Scripts.Data.Converters
{
    public class ConsumePropertyJsonConverter : JsonConverter<ConsumeProperty>
    {
        public override ConsumeProperty ReadJson(JsonReader reader, Type objectType, ConsumeProperty existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            string data = (string)reader.Value;
            string[] args = data.Replace(" ", "").Split('$');

            try
            {
                string[] vals;
                switch (args[0])
                {
                    default: return null;
                    case "Recover":
                        vals = args[1].Split('>');
                        Recover recover = new Recover()
                        {
                            amount = float.Parse(vals[1])
                        };
                        return recover;
                    case "Replenish":
                        vals = args[1].Split('>');
                        Replenish replenish = new Replenish()
                        {
                            amount = float.Parse(vals[1])
                        };
                        return replenish;
                }
            } catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return null;
        }

        public override void WriteJson(JsonWriter writer, ConsumeProperty value, JsonSerializer serializer)
        {
            // [Property Type]$[param1]>[param1 value]$[param2]>[param2 value]...
            writer.WriteValue(value.ToDataString());
        }
    }
}
