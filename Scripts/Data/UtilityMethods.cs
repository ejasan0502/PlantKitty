using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantKitty.Scripts.Data
{
    public static class UtilityMethods
    {
        public static string ToSignString(int val)
        {
            if (val > 0) return "+" + val;
            return val+"";
        }
        public static string ToSignString(float val)
        {
            if (val > 0) return "+" + val;
            return val + "";
        }
    }
}
