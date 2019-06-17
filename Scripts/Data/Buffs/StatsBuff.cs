using System;
using System.Collections.Generic;
using System.Text;

namespace PlantKitty.Scripts.Data
{
    public class StatsBuff : Buff
    {
        public bool percent;
        public Stats stats;

        public StatsBuff() : base()
        {
            percent = false;
            stats = new Stats(0f);
        }

        public override string Description
        {
            get
            {
                string percentSign = percent ? "%" : "";
                string info = "";
                if (stats.HP != 0) info += $"HP: {UtilityMethods.ToSignString(stats.HP)}{percentSign}";
                if (stats.MP != 0) info += (info != "" ? "\n" : "") + $"MP: {UtilityMethods.ToSignString(stats.MP)}{percentSign}";
                if (stats.ATK != 0) info += (info != "" ? "\n" : "") + $"ATK: {UtilityMethods.ToSignString(stats.ATK)}{percentSign}";
                if (stats.DEF != 0) info += (info != "" ? "\n" : "") + $"DEF: {UtilityMethods.ToSignString(stats.DEF)}{percentSign}";
                if (stats.ACC != 0) info += (info != "" ? "\n" : "") + $"ACC: {UtilityMethods.ToSignString(stats.ACC)}{percentSign}";
                if (stats.EVA != 0) info += (info != "" ? "\n" : "") + $"EVA: {UtilityMethods.ToSignString(stats.EVA)}{percentSign}";
                if (stats.SPD != 0) info += (info != "" ? "\n" : "") + $"SPD: {UtilityMethods.ToSignString(stats.SPD)}{percentSign}";

                return info;
            }
        }
        public override void Apply(Player player)
        {
            player.maxStats += stats;
        }
    }
}
