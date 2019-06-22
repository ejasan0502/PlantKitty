using Newtonsoft.Json;
using PlantKitty.Scripts.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlantKitty.Scripts.Statuses
{
    public struct Status
    {
        public string name;
        public bool continuous;
        public int duration;
        public List<StatusProperty> properties;

        private Character caster;

        public string Description()
        {
            string s = "";
            foreach (StatusProperty sp in properties)
            {
                if (s != "") s += "\n";
                s += sp.Description(caster);
            }

            return s;
        }
        public void Apply(Character target)
        {
            foreach (StatusProperty p in properties)
                p.Apply(caster, target);
        }
        public void SetCaster(Character caster)
        {
            this.caster = caster;
        }
        public void Decrement()
        {
            duration--;
        }
    }
}
