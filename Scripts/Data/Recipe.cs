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

        public bool CanCraft(Inventory inventory)
        {
            for (int i = 0; i < materials.Count; i++)
            {
                int amount = i < amounts.Count ? amounts[i] : 1;
                if (!inventory.HasEnoughOf(materials[i], amount))
                    return false;
            }
            return true;
        }
    }
}
