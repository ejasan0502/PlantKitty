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
        public float ATK;
        public float DEF;
        public float ACC;
        public float EVA;
        public float SPD;

        public Stats()
        {
            HP = 0;
            MP = 0;
            ATK = 0;
            DEF = 0;
            ACC = 0;
            EVA = 0;
            SPD = 0;
        }
        public Stats(string data)
        {
            HP = 0;
            MP = 0;
            ATK = 0;
            DEF = 0;
            ACC = 0;
            EVA = 0;
            SPD = 0;

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
            ATK = val;
            DEF = val;
            ACC = val;
            EVA = val;
            SPD = val;
        }
        public Stats(float hp, float mp, float atk, float def, float acc, float eva, float spd)
        {
            HP = hp;
            MP = mp;
            ATK = atk;
            DEF = def;
            ACC = acc;
            EVA = eva;
            SPD = spd;
        }

        public static Stats operator+(Stats s1, Stats s2)
        {
            Stats stats = new Stats();
            stats.HP = s1.HP + s2.HP;
            stats.MP = s1.MP + s2.MP;
            stats.ATK = s1.ATK + s2.ATK;
            stats.DEF = s1.DEF + s2.DEF;
            stats.ACC = s1.ACC + s2.ACC;
            stats.EVA = s1.EVA + s2.EVA;
            stats.SPD = s1.SPD + s2.SPD;

            return stats;
        }
        public static Stats operator -(Stats s1, Stats s2)
        {
            Stats stats = new Stats();
            stats.HP = s1.HP - s2.HP;
            stats.MP = s1.MP - s2.MP;
            stats.ATK = s1.ATK - s2.ATK;
            stats.DEF = s1.DEF - s2.DEF;
            stats.ACC = s1.ACC - s2.ACC;
            stats.EVA = s1.EVA - s2.EVA;
            stats.SPD = s1.SPD - s2.SPD;

            return stats;
        }
        public static Stats operator *(Stats s1, Stats s2)
        {
            Stats stats = new Stats();
            stats.HP = s1.HP * s2.HP;
            stats.MP = s1.MP * s2.MP;
            stats.ATK = s1.ATK * s2.ATK;
            stats.DEF = s1.DEF * s2.DEF;
            stats.ACC = s1.ACC * s2.ACC;
            stats.EVA = s1.EVA * s2.EVA;
            stats.SPD = s1.SPD * s2.SPD;

            return stats;
        }
        public static Stats operator /(Stats s1, Stats s2)
        {
            Stats stats = new Stats();
            stats.HP = s1.HP / s2.HP;
            stats.MP = s1.MP / s2.MP;
            stats.ATK = s1.ATK / s2.ATK;
            stats.DEF = s1.DEF / s2.DEF;
            stats.ACC = s1.ACC / s2.ACC;
            stats.EVA = s1.EVA / s2.EVA;
            stats.SPD = s1.SPD / s2.SPD;

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
    }
}
