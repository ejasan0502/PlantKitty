using Newtonsoft.Json;
using PlantKitty.Scripts.Skills;
using PlantKitty.Scripts.Skills.SkillProperties;
using PlantKitty.Scripts.Statuses;
using PlantKitty.Scripts.Statuses.StatusProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantKitty.Scripts.Data.Converters
{
    public class ListStatusPropertyJsonConverter : JsonConverter<List<StatusProperty>>
    {
        public override bool CanWrite
        {
            get
            {
                return false;
            }
        }

        public override List<StatusProperty> ReadJson(JsonReader reader, Type objectType, List<StatusProperty> existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            string data = (string)reader.Value;
            string[] args = data.Split('|');

            List<StatusProperty> properties = new List<StatusProperty>();
            try
            {
                for (int i = 0; i < args.Length; i++)
                {
                    string[] vals = args[i].Split('$');
                    switch (vals[0])
                    {
                        default: return null;
                        case "Buff_StP":
                            properties.Add(new Buff_StP()
                            {
                                percent = bool.Parse(vals[1].Split('>')[1]),
                                stats = new Stats(vals[2].Split('>')[1])
                            });
                            break;
                        case "DoT_StP":
                            properties.Add(new DoT_StP()
                            {
                                percent = bool.Parse(vals[1].Split('>')[1]),
                                inflict = float.Parse(vals[2].Split('>')[1])
                            });
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

        public override void WriteJson(JsonWriter writer, List<StatusProperty> value, JsonSerializer serializer)
        {
            throw new System.NotImplementedException();
        }
    }
}
