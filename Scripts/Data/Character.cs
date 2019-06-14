using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PlantKitty.Scripts.Data
{
    public abstract class Character
    {
        public string name;
        public Stats currentStats;
        public Stats maxStats;

        public void Hit(float amount)
        {
            currentStats.HP -= amount;
            if (currentStats.HP > maxStats.HP)
                currentStats.HP = maxStats.HP;
            else if (currentStats.HP < 0)
                currentStats.HP = 0;
        }
        public void Replenish(float amount)
        {
            currentStats.MP += amount;
            if (currentStats.MP > maxStats.MP)
                currentStats.MP = maxStats.MP;
            else if (currentStats.MP < 0)
                currentStats.MP = 0;
        }
    }
}
