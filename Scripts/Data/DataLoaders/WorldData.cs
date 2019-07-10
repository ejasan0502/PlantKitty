using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace PlantKitty.Scripts.Data.DataLoaders
{
    public class WorldData : DataLoader<Field>
    {
        public WorldData(string path) : base()
        {
            Load(path);
        }

        public override List<Field> GenerateData(string path)
        {
            List<Field> list = new List<Field>()
            {
                new Field()
                {
                    name = "Field",
                    loot = new List<string>()
                    {
                        "Material",
                        "Consumable",
                        "Weapon"
                    },
                    monsters = new List<string>()
                    {
                        "Dummy"
                    }
                }
            };

            string json = JsonConvert.SerializeObject(list);
            File.WriteAllText(path, json);
            Console.WriteLine("Created test WorldData.json!");
            return list;
        }
        public override void AddDataToDictionary(List<Field> dataList)
        {
            foreach (Field field in dataList)
            {
                data.Add(field.name, field);
            }
            Console.WriteLine("Field data loaded!");
        }
    }
}
