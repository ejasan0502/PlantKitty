using Newtonsoft.Json;
using PlantKitty.Scripts.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantKitty.Scripts.Data.Converters
{
    public class ListSkillJsonConverter : JsonConverter<List<Skill>>
    {
        public override List<Skill> ReadJson(JsonReader reader, Type objectType, List<Skill> existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            List<Skill> values = new List<Skill>();
            string data = (string)reader.Value;
            string[] args = data.Split(',');
            foreach (string arg in args)
            {
                if (arg == "") continue;
                Skill skill = GameData.Instance.GetSkill(arg);
                if (skill == null) continue;
                values.Add(skill);
            }

            return values;
        }

        public override void WriteJson(JsonWriter writer, List<Skill> value, JsonSerializer serializer)
        {
            string data = "";
            foreach (Skill val in value)
            {
                if (data != "") data += ",";
                data += val.name;
            }
            writer.WriteValue(data);
        }
    }
}
