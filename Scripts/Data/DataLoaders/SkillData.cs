using Newtonsoft.Json;
using PlantKitty.Scripts.Skills;
using PlantKitty.Scripts.Skills.SkillProperties;
using System;
using System.Collections.Generic;
using System.IO;

namespace PlantKitty.Scripts.Data.DataLoaders
{
    public class SkillData : DataLoader<Skill>
    {
        public SkillData(string path) : base()
        {
            Load(path);
        }

        public override List<Skill> GenerateData(string path)
        {
            List<Skill> list = new List<Skill>()
            {
                new Skill()
                {
                    name = "Skill 1",
                    isFriendly = false,
                    isAoe = false,
                    properties = new List<SkillProperty>()
                    {
                        new Damage_SP()
                        {
                            percent = false,
                            inflict = 1
                        }
                    }
                }
            };

            string json = JsonConvert.SerializeObject(list);
            File.WriteAllText(path, json);
            Console.WriteLine("Created test SkillData.json!");
            return list;
        }
        public override void AddDataToDictionary(List<Skill> dataList)
        {
            foreach (Skill skill in dataList)
            {
                data.Add(skill.name, skill);
            }
            Console.WriteLine("Skill data loaded!");
        }
    }
}
