using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace PlantKitty.Scripts.Data.DataLoaders
{
    public class RecipeData : DataLoader<Recipe>
    {
        public Dictionary<string, List<string>> recipeItemTypes;

        public RecipeData() : base()
        {
            recipeItemTypes = new Dictionary<string, List<string>>();
        }

        public override List<Recipe> GenerateData(string path)
        {
            List<Recipe> list = new List<Recipe>()
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

            string json = JsonConvert.SerializeObject(list);
            File.WriteAllText(path, json);
            Console.WriteLine("Created test RecipeData.json!");
            return list;
        }
        public override void AddDataToDictionary(List<Recipe> dataList)
        {
            dataList.Sort((x, y) => string.Compare(x.product, y.product));
            for (int i = dataList.Count - 1; i >= 0; i--)
            {
                int index = i;
                Item item = GameData.Instance.GetItem(dataList[index].product);
                if (item == null) continue;

                string itemType = item.GetType().Name.ToLower();
                if (!recipeItemTypes.ContainsKey(itemType)) recipeItemTypes.Add(itemType, new List<string>());
                recipeItemTypes[itemType].Add(dataList[index].product);

                if (!data.ContainsKey(item.name)) data.Add(item.name, dataList[index]);
            }

            Console.WriteLine("Recipe data loaded!");
        }
    }
}
