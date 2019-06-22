using Newtonsoft.Json;
using PlantKitty.Scripts.Statuses;
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

        protected List<Status> statuses;

        public Character()
        {
            statuses = new List<Status>();
        }

        private bool HasStatus(string statusName)
        {
            foreach (Status s in statuses)
                if (s.name == statusName)
                    return true;
            return false;
        }

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

        public void AddStatus(Character caster, string status)
        {
            if (statuses == null) statuses = new List<Status>();

            Status st = GameData.Instance.GetStatus(status);
            if (st.name != "" || HasStatus(status)) return;

            st.SetCaster(caster);
            statuses.Add(st);

            if (!st.continuous)
                st.Apply(this);
        }
        public void RemoveStatus(string status)
        {
            if (statuses == null) statuses = new List<Status>();

            Status st = GameData.Instance.GetStatus(status);
            if (st.name != "" || HasStatus(status)) return;

            for (int i = statuses.Count - 1; i >= 0; i--)
                if (statuses[i].name == status)
                    statuses.RemoveAt(i);
        }
        public void CheckStatuses()
        {
            for (int i = statuses.Count-1; i >= 0; i--)
            {
                if (statuses[i].continuous)
                    statuses[i].Apply(this);

                statuses[i].Decrement();
                if (statuses[i].duration < 0)
                    statuses.RemoveAt(i);
            }
        }
        public void ClearStatuses()
        {
            statuses = new List<Status>();
        }
    }
}
