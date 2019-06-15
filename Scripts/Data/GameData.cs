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
        private const string RecipeDataPath = "Resources/RecipeData.json";

        private Dictionary<string, Item> items;
        private Dictionary<string, Field> fields;
        private Dictionary<string, Monster> monsters;

        private List<Recipe> allRecipes;
        private Dictionary<string, List<int>> recipeItemTypes;
        private Dictionary<string, int> recipes;

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

            recipeItemTypes = new Dictionary<string, List<int>>();
            recipes = new Dictionary<string, int>();

            LoadData();
        }

        private void LoadData()
        {
            LoadItemData();
            LoadWorldData();
            LoadMonsterData();
            LoadRecipeData();
        }
        private void LoadItemData()
        {
            List<Item> itemList = new List<Item>();
            if (!File.Exists(ItemDataPath))
            {
                itemList = new List<Item>()
                {
                    new Item("Material")
                    {
                        tier = Tier.common,
                        lootCategory = LootCategory.foraging
                    },
                    new Equip("Weapon", EquipType.primary, new Stats(1f)),
                    new Consumable("Consumable", true, new Recover(), new Replenish())
                };

                string json = JsonConvert.SerializeObject(itemList);
                File.WriteAllText(ItemDataPath, json);
                Console.WriteLine("Created test ItemData.json!");
            } else 
                itemList = JsonConvert.DeserializeObject<List<Item>>(File.ReadAllText(ItemDataPath));

            foreach (Item item in itemList)
            {
                items.Add(item.name, item);
            }
            Console.WriteLine("Item data loaded!");
        }
        private void LoadWorldData()
        {
            List<Field> fieldList = new List<Field>();
            if (!File.Exists(WorldDataPath))
            {
                fieldList = new List<Field>()
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

                string json = JsonConvert.SerializeObject(fieldList);
                File.WriteAllText(WorldDataPath, json);
                Console.WriteLine("Created test WorldData.json!");
            } else
                fieldList = JsonConvert.DeserializeObject<List<Field>>(File.ReadAllText(WorldDataPath));

            foreach (Field field in fieldList)
            {
                fields.Add(field.name, field);
            }
            Console.WriteLine("World data loaded!");
        }
        private void LoadMonsterData()
        {
            List<Monster> monsterList = new List<Monster>();
            if (!File.Exists(MonsterDataPath))
            {
                monsterList = new List<Monster>()
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

                string json = JsonConvert.SerializeObject(monsterList);
                File.WriteAllText(MonsterDataPath, json);
                Console.WriteLine("Created MonsterData.json!");
            } else
                monsterList = JsonConvert.DeserializeObject<List<Monster>>(File.ReadAllText(MonsterDataPath));

            foreach (Monster monster in monsterList)
            {
                monsters.Add(monster.name, monster);
            }
            Console.WriteLine("Monster data loaded!");
        }
        private void LoadRecipeData()
        {
            if (File.Exists(RecipeDataPath))
            {
                allRecipes = JsonConvert.DeserializeObject<List<Recipe>>(File.ReadAllText(RecipeDataPath));
                Console.WriteLine("Recipe data loaded!");
            }
            else
            {
                allRecipes = new List<Recipe>()
                {
                    new Recipe()
                    {
                        product = "Lesser HP Potion",
                        time = 10,
                        materials = new List<string>()
                        {
                            "Gooseberries"
                        },
                        amounts = new List<int>()
                        {
                            1
                        }
                    }
                };

                string json = JsonConvert.SerializeObject(allRecipes);
                File.WriteAllText(RecipeDataPath, json);
                Console.WriteLine("Created RecipeData.json!");
            }

            allRecipes.Sort((x, y) => string.Compare(x.product, y.product));
            for (int i = allRecipes.Count-1; i >= 0; i--)
            {
                int index = i;
                Item item = GetItem(allRecipes[index].product);
                if (item == null) continue;

                string itemType = item.GetType().Name.ToLower();
                if (!recipeItemTypes.ContainsKey(itemType)) recipeItemTypes.Add(itemType, new List<int>());
                recipeItemTypes[itemType].Add(index);

                if (!recipes.ContainsKey(item.name)) recipes.Add(item.name, index);
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
        public List<Recipe> GetRecipes(string itemType)
        {
            if (!recipeItemTypes.ContainsKey(itemType)) return null;

            List<Recipe> recipeList = new List<Recipe>();
            foreach (int i in recipeItemTypes[itemType])
                recipeList.Add(allRecipes[i]);

            return recipeList;
        }
        public Recipe GetRecipe(string itemName)
        {
            if (recipes.ContainsKey(itemName))
                return allRecipes[ recipes[itemName] ];
            return null;
        }
    }
}
