using Discord;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantKitty.Scripts.Data
{
    public class Item
    {
        public string name;
        public Tier tier;
        public LootCategory lootCategory;

        [JsonIgnore]
        public virtual string Description
        {
            get
            {
                return $"Tier: {tier.ToString()}";
            }
        }

        public Item(string name)
        {
            this.name = name;
            tier = Tier.common;
            lootCategory = LootCategory.foraging;
        }

        public Color GetColor()
        {
            switch (tier)
            {
                case Tier.common: return Color.Default;
                case Tier.uncommon: return Color.Green;
                case Tier.rare: return Color.Blue;
                case Tier.unique: return Color.Gold;
            }
            return Color.Default;
        }
    }

    public enum Tier
    {
        common = 10,
        uncommon = 7,
        rare = 4,
        unique = 1
    }
    public enum LootCategory
    {
        foraging,
        hunting,
        fishing,
        mining,
        battling
    }
}
