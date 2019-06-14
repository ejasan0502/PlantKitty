using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PlantKitty.Scripts.Data
{
    public class Attributes
    {
        public int STR, VIT, DEX, AGI, INT, PSY, LUK;

        public Attributes()
        {
            STR = VIT = DEX = AGI = INT = PSY = LUK = 0;
        }
        public Attributes(string data)
        {
            STR = VIT = DEX = AGI = INT = PSY = LUK = 0;

            string[] args = data.Replace(" ", "").Split(',');
            foreach (string arg in args)
            {
                string[] vals = arg.Split('=');
                FieldInfo field = GetType().GetField(vals[0]);

                if (field != null && int.TryParse(vals[1], out int val))
                    field.SetValue(this, val);
            }
        }
        public Attributes(int val)
        {
            STR = VIT = DEX = AGI = INT = PSY = LUK = val;
        }
        public Attributes(int str, int vit, int dex, int agi, int inT, int psy, int luk)
        {
            STR = str;
            VIT = vit;
            DEX = dex;
            AGI = agi;
            INT = inT;
            PSY = psy;
            LUK = luk;
        }

        public static Attributes operator+(Attributes a1, Attributes a2)
        {
            Attributes a = new Attributes();
            a.STR = a1.STR + a2.STR;
            a.VIT = a1.VIT + a2.VIT;
            a.DEX = a1.DEX + a2.DEX;
            a.AGI = a1.AGI + a2.AGI;
            a.INT = a1.INT + a2.INT;
            a.PSY = a1.PSY + a2.PSY;
            a.LUK = a1.LUK + a2.LUK;

            return a;
        }
        public static Attributes operator -(Attributes a1, Attributes a2)
        {
            Attributes a = new Attributes();
            a.STR = a1.STR - a2.STR;
            a.VIT = a1.VIT - a2.VIT;
            a.DEX = a1.DEX - a2.DEX;
            a.AGI = a1.AGI - a2.AGI;
            a.INT = a1.INT - a2.INT;
            a.PSY = a1.PSY - a2.PSY;
            a.LUK = a1.LUK - a2.LUK;

            return a;
        }
        public static Attributes operator *(Attributes a1, Attributes a2)
        {
            Attributes a = new Attributes();
            a.STR = a1.STR * a2.STR;
            a.VIT = a1.VIT * a2.VIT;
            a.DEX = a1.DEX * a2.DEX;
            a.AGI = a1.AGI * a2.AGI;
            a.INT = a1.INT * a2.INT;
            a.PSY = a1.PSY * a2.PSY;
            a.LUK = a1.LUK * a2.LUK;

            return a;
        }
        public static Attributes operator /(Attributes a1, Attributes a2)
        {
            Attributes a = new Attributes();
            a.STR = a1.STR / a2.STR;
            a.VIT = a1.VIT / a2.VIT;
            a.DEX = a1.DEX / a2.DEX;
            a.AGI = a1.AGI / a2.AGI;
            a.INT = a1.INT / a2.INT;
            a.PSY = a1.PSY / a2.PSY;
            a.LUK = a1.LUK / a2.LUK;

            return a;
        }

        public override string ToString()
        {
            string args = "";
            FieldInfo[] fields = GetType().GetFields();
            foreach (FieldInfo field in fields)
            {
                int val = (int)field.GetValue(this);
                if (args != "") args += ",";
                args += field.Name + "=" + val;
            }

            return args;
        }
    }
}
