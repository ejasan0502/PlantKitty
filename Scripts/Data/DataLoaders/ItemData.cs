using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace PlantKitty.Scripts.Data.DataLoaders
{
    public class ItemData : DataLoader<Item>
    {
        public ItemData(string path) : base()
        {
            Load(path);
        }

        public override List<Item> GenerateData(string path)
        {
            List<Item> list = new List<Item>()
            {
                new Item("Material")
                {
                    tier = Tier.common,
                    lootCategory = LootCategory.foraging
                },
                new Equip("Weapon", EquipType.primary, new Stats(1f)),
                new Consumable("Consumable", true, new Recover(), new Replenish())
            };

            string json = JsonConvert.SerializeObject(list);
            File.WriteAllText(path, json);
            Console.WriteLine("Created test ItemData.json!");
            return list;
        }
        public override void AddDataToDictionary(List<Item> dataList)
        {
            foreach (Item item in dataList)
            {
                data.Add(item.name, item);
            }
            Console.WriteLine("Item data loaded!");
        }
    }
}
