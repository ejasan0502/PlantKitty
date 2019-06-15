using Newtonsoft.Json;
using PlantKitty.Scripts.Data.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlantKitty.Scripts.Data
{
    public class Recipe
    {
        public string product;
        public int time;
        [JsonConverter(typeof(ListStringJsonConverter))] public List<string> materials;
        [JsonConverter(typeof(ListIntJsonConverter))] public List<int> amounts;

        [JsonIgnore]
        public string Ingredients
        {
            get
            {
                string s = "";
                for (int i = 0; i < materials.Count; i++)
                {
                    if (i != 0) s += "\n";
                    int amt = i < amounts.Count ? amounts[i] : 1;
                    s += $"x{amt} {materials[i]}";
                }

                return s;
            }
        }

        public bool CanCraft(Inventory inventory, int count = 1)
        {
            for (int i = 0; i < materials.Count; i++)
            {
                int amount = i < amounts.Count ? amounts[i] : 1;
                if (!inventory.HasEnoughOf(materials[i], amount*count))
                    return false;
            }
            return true;
        }
    }
}
