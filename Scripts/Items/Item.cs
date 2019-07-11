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
        public int buyValue;
        public int sellValue;

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
            buyValue = 0;
            sellValue = 0;
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
        public override bool Equals(object obj)
        {
            return obj is Item && ((Item)obj).name == name;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public enum ItemType
    {
        item,
        equip,
        consumable
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
        none,
        foraging,
        hunting,
        fishing,
        mining,
        battling
    }
}
