using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PlantKitty.Scripts.Data
{
    public class Equip : Item
    {
        public EquipType equipType;
        public Stats stats;

        public override string Description
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                builder.Append($"{equipType.ToString()}\n");
                if (stats.HP != 0) builder.Append($"HP: {stats.HP}\n");
                if (stats.MP != 0) builder.Append($"MP: {stats.MP}\n");
                if (stats.ATK != 0) builder.Append($"ATK: {stats.ATK}\n");
                if (stats.DEF != 0) builder.Append($"DEF: {stats.DEF}\n");
                if (stats.ACC != 0) builder.Append($"ACC: {stats.ACC}\n");
                if (stats.EVA != 0) builder.Append($"EVA: {stats.EVA}");

                return builder.ToString();
            }
        }

        public Equip(string name, EquipType equipType, Stats stats) : base(name)
        {
            this.equipType = equipType;
            this.stats = stats;
        }
    }

    public enum EquipType
    {
        primary = 0,
        secondary = 1,
        helm = 2,
        chest = 3,
        pants = 4,
        boots = 5,
        gloves = 6,
        ring = 7,
        necklace = 8,
        bracelet = 9,

        twoHand = 10,
        overall = 11
    }
}
