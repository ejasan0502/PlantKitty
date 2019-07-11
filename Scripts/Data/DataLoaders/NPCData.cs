using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace PlantKitty.Scripts.Data.DataLoaders
{
    public class NPCData : DataLoader<NPC>
    {
        public NPCData(string path) : base()
        {
            Load(path);
        }

        public override List<NPC> GenerateData(string path)
        {
            List<NPC> list = new List<NPC>()
            {
                new MerchantNPC()
                {
                    name = "Merchant",
                    sellItems = new List<string>()
                    {
                        "Gooseberries",
                        "Sage Herb"
                    }
                }
            };

            string json = JsonConvert.SerializeObject(list);
            File.WriteAllText(path, json);
            Console.WriteLine("Created test NPCData.json!");
            return list;
        }
        public override void AddDataToDictionary(List<NPC> dataList)
        {
            foreach (NPC npc in dataList)
            {
                data.Add(npc.name, npc);
            }
            Console.WriteLine("NPC data loaded!");
        }
    }
}
