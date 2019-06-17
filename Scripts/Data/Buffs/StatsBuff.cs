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
                string info =  $"HP: {UtilityMethods.ToSignString(stats.HP)}{percentSign}\n" +
                               $"MP: {UtilityMethods.ToSignString(stats.MP)}{percentSign}\n" +
                               $"ATK: {UtilityMethods.ToSignString(stats.ATK)}{percentSign}\n" +
                               $"DEF: {UtilityMethods.ToSignString(stats.DEF)}{percentSign}\n" +
                               $"ACC: {UtilityMethods.ToSignString(stats.ACC)}{percentSign}\n" +
                               $"EVA: {UtilityMethods.ToSignString(stats.EVA)}{percentSign}\n" +
                               $"SPD: {UtilityMethods.ToSignString(stats.SPD)}{percentSign}";

                return info;
            }
        }
        public override void Apply(Player player)
        {
            player.maxStats += stats;
        }
    }
}
