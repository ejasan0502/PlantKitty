using System;
using System.Collections.Generic;
using System.Text;
using PlantKitty.Scripts.Data;

namespace PlantKitty.Scripts.Statuses.StatusProperties
{
    public class Buff_StP : StatusProperty
    {
        public bool percent;
        public Stats stats;

        public override string Description(Character caster)
        {
            string p = percent ? "%" : "";

            string s = "";
            if (stats.HP != 0) s += (s != "" ? "\n" : "") + $"HP: {stats.HP}{p}";
            if (stats.MP != 0) s += (s != "" ? "\n" : "") + $"MP: {stats.MP}{p}";
            if (stats.ATK != 0) s += (s != "" ? "\n" : "") + $"ATK: {stats.ATK}{p}";
            if (stats.DEF != 0) s += (s != "" ? "\n" : "") + $"DEF: {stats.DEF}{p}";
            if (stats.ACC != 0) s += (s != "" ? "\n" : "") + $"ACC: {stats.ACC}{p}";
            if (stats.EVA != 0) s += (s != "" ? "\n" : "") + $"EVA: {stats.EVA}{p}";
            if (stats.SPD != 0) s += (s != "" ? "\n" : "") + $"SPD: {stats.SPD}{p}";
            return s;
        }
        public override void Apply(Character caster, Character target)
        {
            if (percent)
                target.maxStats *= stats;
            else
                target.maxStats += stats;
        }
    }
}
