using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PlantKitty.Scripts.Data
{
    public class Stats
    {
        public float HP;
        public float MP;
        public float PATK;
        public float MATK;
        public float PDEF;
        public float MDEF;
        public float ACC;
        public float EVA;
        public float SPD;
        public float CRIT;
        public float CRITDMG;

        public Stats()
        {
            HP = 0;
            MP = 0;
            PATK = 0;
            MATK = 0;
            PDEF = 0;
            MDEF = 0;
            ACC = 0;
            EVA = 0;
            SPD = 0;
            CRIT = 0;
            CRITDMG = 0;
        }
        public Stats(Stats stats)
        {
            HP = stats.HP;
            MP = stats.MP;
            PATK = stats.PATK;
            PDEF = stats.PDEF;
            MATK = stats.MATK;
            MDEF = stats.MDEF;
            ACC = stats.ACC;
            EVA = stats.EVA;
            SPD = stats.SPD;
            CRIT = stats.CRIT;
            CRITDMG = stats.CRITDMG;
        }
        public Stats(string data)
        {
            HP = 0;
            MP = 0;
            PATK = 0;
            MATK = 0;
            PDEF = 0;
            MDEF = 0;
            ACC = 0;
            EVA = 0;
            SPD = 0;
            CRIT = 0;
            CRITDMG = 0;

            string[] args = data.Replace(" ", "").Split(',');
            foreach (string arg in args)
            {
                string[] vals = arg.Split('=');
                FieldInfo field = GetType().GetField(vals[0]);

                float val;
                if (field != null && float.TryParse(vals[1], out val))
                    field.SetValue(this, val);
            }
        }
        public Stats(float val)
        {
            HP = val;
            MP = val;
            PATK = val;
            MATK = val;
            PDEF = val;
            MDEF = val;
            ACC = val;
            EVA = val;
            SPD = val;
            CRIT = val;
            CRITDMG = val;
        }

        public static Stats operator+(Stats s1, Stats s2)
        {
            Stats stats = new Stats();
            stats.HP = s1.HP + s2.HP;
            stats.MP = s1.MP + s2.MP;
            stats.PATK = s1.PATK + s2.PATK;
            stats.PDEF = s1.PDEF + s2.PDEF;
            stats.MATK = s1.MATK + s2.MATK;
            stats.MDEF = s1.MDEF + s2.MDEF;
            stats.ACC = s1.ACC + s2.ACC;
            stats.EVA = s1.EVA + s2.EVA;
            stats.SPD = s1.SPD + s2.SPD;
            stats.CRIT = s1.CRIT + s2.CRIT;
            stats.CRITDMG = s1.CRITDMG + s2.CRITDMG;

            return stats;
        }
        public static Stats operator -(Stats s1, Stats s2)
        {
            Stats stats = new Stats();
            stats.HP = s1.HP - s2.HP;
            stats.MP = s1.MP - s2.MP;
            stats.PATK = s1.PATK - s2.PATK;
            stats.PDEF = s1.PDEF - s2.PDEF;
            stats.MATK = s1.MATK - s2.MATK;
            stats.MDEF = s1.MDEF - s2.MDEF;
            stats.ACC = s1.ACC - s2.ACC;
            stats.EVA = s1.EVA - s2.EVA;
            stats.SPD = s1.SPD - s2.SPD;
            stats.CRIT = s1.CRIT - s2.CRIT;
            stats.CRITDMG = s1.CRITDMG - s2.CRITDMG;

            return stats;
        }
        public static Stats operator *(Stats s1, Stats s2)
        {
            Stats stats = new Stats();
            stats.HP = s1.HP * s2.HP;
            stats.MP = s1.MP * s2.MP;
            stats.PATK = s1.PATK * s2.PATK;
            stats.PDEF = s1.PDEF * s2.PDEF;
            stats.MATK = s1.MATK * s2.MATK;
            stats.MDEF = s1.MDEF * s2.MDEF;
            stats.ACC = s1.ACC * s2.ACC;
            stats.EVA = s1.EVA * s2.EVA;
            stats.SPD = s1.SPD * s2.SPD;
            stats.CRIT = s1.CRIT * s2.CRIT;
            stats.CRITDMG = s1.CRITDMG * s2.CRITDMG;

            return stats;
        }
        public static Stats operator /(Stats s1, Stats s2)
        {
            Stats stats = new Stats();
            stats.HP = s1.HP / s2.HP;
            stats.MP = s1.MP / s2.MP;
            stats.PATK = s1.PATK / s2.PATK;
            stats.PDEF = s1.PDEF / s2.PDEF;
            stats.MATK = s1.MATK / s2.MATK;
            stats.MDEF = s1.MDEF / s2.MDEF;
            stats.ACC = s1.ACC / s2.ACC;
            stats.EVA = s1.EVA / s2.EVA;
            stats.SPD = s1.SPD / s2.SPD;
            stats.CRIT = s1.CRIT / s2.CRIT;
            stats.CRITDMG = s1.CRITDMG / s2.CRITDMG;

            return stats;
        }

        public override string ToString()
        {
            string args = "";
            FieldInfo[] fields = GetType().GetFields();
            foreach (FieldInfo field in fields)
            {
                float val = (float)field.GetValue(this);
                if (args != "") args += ",";
                args += field.Name + "=" + val;
            }

            return args;
        }
        public string ToStringWithSigns(bool percent)
        {
            string percentSign = percent ? "%" : "";
            string info = "";
            if (HP != 0) info += $"HP: {UtilityMethods.ToSignString(HP)}{percentSign}";
            if (MP != 0) info += (info != "" ? "\n" : "") + $"MP: {UtilityMethods.ToSignString(MP)}{percentSign}";
            if (PATK != 0) info += (info != "" ? "\n" : "") + $"PATK: {UtilityMethods.ToSignString(PATK)}{percentSign}";
            if (PDEF != 0) info += (info != "" ? "\n" : "") + $"PDEF: {UtilityMethods.ToSignString(PDEF)}{percentSign}";
            if (MATK != 0) info += (info != "" ? "\n" : "") + $"MATK: {UtilityMethods.ToSignString(MATK)}{percentSign}";
            if (MDEF != 0) info += (info != "" ? "\n" : "") + $"MDEF: {UtilityMethods.ToSignString(MDEF)}{percentSign}";
            if (ACC != 0) info += (info != "" ? "\n" : "") + $"ACC: {UtilityMethods.ToSignString(ACC)}{percentSign}";
            if (EVA != 0) info += (info != "" ? "\n" : "") + $"EVA: {UtilityMethods.ToSignString(EVA)}{percentSign}";
            if (SPD != 0) info += (info != "" ? "\n" : "") + $"SPD: {UtilityMethods.ToSignString(SPD)}{percentSign}";
            if (CRIT != 0) info += (info != "" ? "\n" : "") + $"CRIT: {UtilityMethods.ToSignString(CRIT)}{percentSign}";
            if (CRITDMG != 0) info += (info != "" ? "\n" : "") + $"CRITDMG: {UtilityMethods.ToSignString(CRITDMG)}{percentSign}";

            return info;
        }
    }
}
