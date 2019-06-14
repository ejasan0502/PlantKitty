using Newtonsoft.Json;
using PlantKitty.Scripts.Combat;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantKitty.Scripts.Data
{
    public class GameData
    {
        private const string ItemDataPath = "Resources/ItemData.json";
        private const string WorldDataPath = "Resources/WorldData.json";
        private const string MonsterDataPath = "Resources/MonsterData.json";

        private Dictionary<string, Item> items;
        private Dictionary<string, Field> fields;
        private Dictionary<string, Monster> monsters;

        private static GameData instance;
        public static GameData Instance
        {
            get
            {
                if ( instance == null)
                {
                    instance = new GameData();
                }
                return instance;
            }
        }

        private GameData()
        {
            items = new Dictionary<string, Item>();
            fields = new Dictionary<string, Field>();
            monsters = new Dictionary<string, Monster>();

            LoadData();
        }

        private void LoadData()
        {
            LoadItemData();
            LoadWorldData();
            LoadMonsterData();
        }
        private void LoadItemData()
        {
            if (File.Exists(ItemDataPath))
            {
                List<Item> loadedItems = JsonConvert.DeserializeObject<List<Item>>(File.ReadAllText(ItemDataPath));
                foreach (Item item in loadedItems)
                {
                    items.Add(item.name, item);
                }
                Console.WriteLine("Item data loaded!");
            } else
            {
                List<Item> generatedItems = new List<Item>()
                {
                    new Item("Material")
                    {
                        tier = Tier.common,
                        lootCategory = LootCategory.foraging
                    },
                    new Equip("Weapon", EquipType.primary, new Stats(1f)),
                    new Consumable("Consumable", true, new Recover(), new Replenish())
                };

                string json = JsonConvert.SerializeObject(generatedItems);
                File.WriteAllText(ItemDataPath, json);
                Console.WriteLine("Created test ItemData.json!");
            }
        }
        private void LoadWorldData()
        {
            if (File.Exists(WorldDataPath))
            {
                List<Field> loadedFields = JsonConvert.DeserializeObject<List<Field>>(File.ReadAllText(WorldDataPath));
                foreach (Field field in loadedFields)
                {
                    fields.Add(field.name, field);
                }
                Console.WriteLine("World data loaded!");
            } else
            {
                List<Field> generatedFields = new List<Field>()
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

                string json = JsonConvert.SerializeObject(generatedFields);
                File.WriteAllText(WorldDataPath, json);
                Console.WriteLine("Created test WorldData.json!");
            }
        }
        private void LoadMonsterData()
        {
            if (File.Exists(MonsterDataPath))
            {
                List<Monster> loadedMonsters = JsonConvert.DeserializeObject<List<Monster>>(File.ReadAllText(MonsterDataPath));
                foreach (Monster monster in loadedMonsters)
                {
                    monsters.Add(monster.name, monster);
                }
                Console.WriteLine("Monster data loaded!");
            } else
            {
                List<Monster> generatedMonsters = new List<Monster>()
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

                string json = JsonConvert.SerializeObject(generatedMonsters);
                File.WriteAllText(MonsterDataPath, json);
                Console.WriteLine("Created MonsterData.json!");
            }
        }

        public Field GetField(string fieldName)
        {
            if (fields.ContainsKey(fieldName))
                return fields[fieldName];

            Console.WriteLine($"Unable to find field named {fieldName}");
            return null;
        }
        public List<Field> GetFields()
        {
            return fields.Values.ToList();
        }
        public Item GetItem(string itemName)
        {
            if (items.ContainsKey(itemName))
                return items[itemName];
            return null;
        }
        public Item GetRandomItem(string fieldName, LootCategory lootCategory)
        {
            Field field = GetField(fieldName);
            if (field == null) return null;

            Tier tier = Tier.common;
            Random random = new Random();
            int percent = random.Next(0, 100);
            if (percent >= 95) tier = Tier.unique;
            else if (percent >= 80) tier = Tier.rare;
            else if (percent >= 50) tier = Tier.uncommon;

            List<Item> lootTable = new List<Item>();
            foreach (string l in field.loot)
            {
                Item item = GetItem(l);
                if (item != null && item.lootCategory == lootCategory && item.tier >= tier)
                {
                    lootTable.Add(item);
                }
            }
            if (lootTable.Count > 0)
            {
                return lootTable[random.Next(lootTable.Count)];
            }

            return null;
        }
        public Monster GetMonster(string monsterName)
        {
            if (monsters.ContainsKey(monsterName))
                return new Monster(monsters[monsterName]);
            return null;
        }
    }
}
