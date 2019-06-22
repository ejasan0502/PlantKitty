using Newtonsoft.Json;
using PlantKitty.Scripts.Skills;
using PlantKitty.Scripts.Skills.SkillProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantKitty.Scripts.Data.Converters
{
    public class ListSkillPropertyJsonConverter : JsonConverter<List<SkillProperty>>
    {
        public override List<SkillProperty> ReadJson(JsonReader reader, Type objectType, List<SkillProperty> existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            string data = (string)reader.Value;
            string[] args = data.Split('|');

            List<SkillProperty> properties = new List<SkillProperty>();
            try
            {
                for (int i = 0; i < args.Length; i++)
                {
                    string[] vals = args[i].Split('$');
                    switch (vals[0])
                    {
                        default: return null;
                        case "Damage_SP":
                            properties.Add(new Damage_SP()
                            {
                                percent = bool.Parse(vals[1].Split('>')[1]),
                                inflict = float.Parse(vals[2].Split('>')[1])
                            });
                            break;
                        case "Status_SP":
                            properties.Add(new Status_SP()
                            {
                                chance = float.Parse(vals[1].Split('>')[1]),
                                status = vals[2].Split('>')[1]
                            });
                            break;
                        case "Heal_SP":
                            properties.Add(new Heal_SP()
                            {
                                percent = bool.Parse(vals[1].Split('>')[1]),
                                amount = float.Parse(vals[2].Split('>')[1])
                            });
                            break;
                        case "Cure_SP":
                            Cure_SP cure = new Cure_SP();
                            foreach (string val in vals[1].Split('>')[1].Split(','))
                            {
                                cure.statuses.Add(val);
                            }

                            properties.Add(cure);
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

        public override void WriteJson(JsonWriter writer, List<SkillProperty> value, JsonSerializer serializer)
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
