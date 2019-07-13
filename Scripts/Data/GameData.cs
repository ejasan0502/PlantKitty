using Newtonsoft.Json;
using PlantKitty.Scripts.Combat;
using PlantKitty.Scripts.Data.DataLoaders;
using PlantKitty.Scripts.Skills;
using PlantKitty.Scripts.Skills.SkillProperties;
using PlantKitty.Scripts.Statuses;
using PlantKitty.Scripts.Statuses.StatusProperties;
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
        private const string JobDataPath = "Resources/JobData.json";
        private const string SkillDataPath = "Resources/SkillData.json";
        private const string StatusDataPath = "Resources/StatusData.json";
        private const string NPCDataPath = "Resources/NPCData.json";

        private ItemData itemData;
        private WorldData worldData;
        private MonsterData monsterData;
        private RecipeData recipeData;
        private JobData jobData;
        private SkillData skillData;
        private StatusData statusData;
        private NPCData npcData;

        private static object padlock = new object();
        private static GameData instance;
        public static GameData Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new GameData();
                    }
                    return instance;
                }
            }
        }

        private GameData()
        {
            itemData = new ItemData();
            worldData = new WorldData();
            statusData = new StatusData();
            skillData = new SkillData();
            recipeData = new RecipeData();
            monsterData = new MonsterData();
            jobData = new JobData();
            npcData = new NPCData();
        }

        public async Task Load()
        {
            await itemData.Load(ItemDataPath);
            await statusData.Load(StatusDataPath);
            await skillData.Load(SkillDataPath);
            await recipeData.Load(RecipeDataPath);
            await monsterData.Load(MonsterDataPath);
            await jobData.Load(JobDataPath);
            await npcData.Load(NPCDataPath);
            await worldData.Load(WorldDataPath);
        }

        public Field GetField(string fieldName)
        {
            if (worldData.data.ContainsKey(fieldName))
                return worldData.data[fieldName];

            Console.WriteLine($"Unable to find field named {fieldName}");
            return null;
        }
        public List<Field> GetFields()
        {
            return worldData.data.Values.ToList();
        }
        public Item GetItem(string itemName)
        {
            if (itemData.data.ContainsKey(itemName))
                return itemData.data[itemName];

            Console.WriteLine($"GetItem returned null! {itemName}");
            return null;
        }
        public Item GetRandomItem(string fieldName, LootCategory lootCategory)
        {
            Field field = GetField(fieldName);
            if (field == null) return null;

            return GetRandomItem(field.loot, lootCategory, 0);
        }
        public Item GetRandomItem(List<string> items, LootCategory? lootCategory, int luck)
        {
            int percentAdd = (int)Math.Round(MathF.Log(MathF.Pow(10 * luck, 2)));

            Tier tier = Tier.common;
            Random random = new Random();
            int percent = random.Next(0, 100) + percentAdd;
            if (percent >= 95) tier = Tier.unique;
            else if (percent >= 80) tier = Tier.rare;
            else if (percent >= 50) tier = Tier.uncommon;

            List<Item> lootTable = new List<Item>();
            foreach (string l in items)
            {
                Item item = GetItem(l);
                if (item != null && item.tier >= tier && (lootCategory == null || item.lootCategory == lootCategory))
                {
                    lootTable.Add(item);
                }
            }
            if (lootTable.Count > 0)
            {
                return lootTable[random.Next(lootTable.Count)];
            }

            Console.WriteLine("GetRandomItem(List, LootCategory, int) returned null!");
            return null;
        }
        public Monster GetMonster(string monsterName)
        {
            if (monsterData.data.ContainsKey(monsterName))
                return new Monster(monsterData.data[monsterName]);

            Console.WriteLine("GetMonster returned null! " + monsterName);
            return null;
        }
        public List<Recipe> GetRecipes(string itemType)
        {
            if (!recipeData.recipeItemTypes.ContainsKey(itemType))
            {
                Console.WriteLine("GetRecipes returned null!");
                return null;
            }

            List<Recipe> recipeList = new List<Recipe>();
            foreach (string i in recipeData.recipeItemTypes[itemType])
                recipeList.Add(recipeData.data[i]);

            return recipeList;
        }
        public Recipe GetRecipe(string itemName)
        {
            if (recipeData.data.ContainsKey(itemName))
                return recipeData.data[itemName];

            Console.WriteLine("GetRecipe returned null! " + itemName);
            return null;
        }
        public JobClass GetJob(string job)
        {
            if (jobData.data.ContainsKey(job))
                return jobData.data[job];

            Console.WriteLine("GetJob returned null! " + job);
            return default;
        }
        public Skill GetSkill(string skill)
        {
            if (skillData.data.ContainsKey(skill))
                return skillData.data[skill];

            Console.WriteLine("GetSkill returned null! " + skill);
            return null;
        }
        public Status GetStatus(string status)
        {
            if (statusData.data.ContainsKey(status))
                return statusData.data[status];

            Console.WriteLine("GetStatus returned null! " + status);
            return default;
        }
        public NPC GetNPC(string npcName)
        {
            if (npcData.data.ContainsKey(npcName))
                return npcData.data[npcName];
            return null;
        }
    }
}
