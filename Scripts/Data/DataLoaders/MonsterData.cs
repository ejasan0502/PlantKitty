using Newtonsoft.Json;
using PlantKitty.Scripts.Combat;
using System;
using System.Collections.Generic;
using System.IO;

namespace PlantKitty.Scripts.Data.DataLoaders
{
    public class MonsterData : DataLoader<Monster>
    {
        public MonsterData(string path) : base()
        {
            Load(path);
        }

        public override List<Monster> GenerateData(string path)
        {
            List<Monster> list = new List<Monster>()
            {
                new Monster()
                {
                    name = "Dummy",
                    currentStats = new Stats(1f),
                    maxStats = new Stats(1f),
                    exp = 1,
                    loot = new List<string>()
                    {
                        "Material",
                        "Weapon",
                        "Consumable"
                    }
                }
            };

            string json = JsonConvert.SerializeObject(list);
            File.WriteAllText(path, json);
            Console.WriteLine("Created test WorldData.json!");
            return list;
        }
        public override void AddDataToDictionary(List<Monster> dataList)
        {
            foreach (Monster monster in dataList)
            {
                data.Add(monster.name, monster);
            }
            Console.WriteLine("Monster data loaded!");
        }
    }
}
