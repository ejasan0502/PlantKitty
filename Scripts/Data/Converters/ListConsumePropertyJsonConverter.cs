using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantKitty.Scripts.Data.Converters
{
    public class ListConsumePropertyJsonConverter : JsonConverter<List<ConsumeProperty>>
    {
        public override List<ConsumeProperty> ReadJson(JsonReader reader, Type objectType, List<ConsumeProperty> existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            string data = (string)reader.Value;
            string[] args = data.Split('|');

            List<ConsumeProperty> properties = new List<ConsumeProperty>();
            try
            {
                for (int i = 0; i < args.Length; i++)
                {
                    string[] vals = args[i].Split('$');
                    string[] param;
                    switch (vals[0])
                    {
                        default: return null;
                        case "Recover":
                            param = vals[1].Split('>');
                            Recover recover = new Recover()
                            {
                                amount = float.Parse(param[1])
                            };
                            properties.Add(recover);
                            break;
                        case "Replenish":
                            param = vals[1].Split('>');
                            Replenish replenish = new Replenish()
                            {
                                amount = float.Parse(param[1])
                            };
                            properties.Add(replenish);
                            break;
                        case "StatsBuff_CP":
                            properties.Add(new StatsBuff_CP(vals));
                            break;
                        case "JobChange":
                            param = vals[1].Split('>');
                            JobChange jobChange = new JobChange()
                            {
                                job = param[1]
                            };
                            properties.Add(jobChange);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return properties;
        }

        public override void WriteJson(JsonWriter writer, List<ConsumeProperty> value, JsonSerializer serializer)
        {
            string s = "";
            for (int i = 0; i < value.Count; i++)
            {
                if (i != 0) s += "|";
                s += value[i].ToDataString();
            }
            writer.WriteValue(s);
        }
    }
}
