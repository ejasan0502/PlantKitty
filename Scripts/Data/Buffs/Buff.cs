using System;
using System.Collections.Generic;
using System.Text;

namespace PlantKitty.Scripts.Data
{
    public abstract class Buff
    {
        public double duration;

        private DateTime start;

        public bool IsDone
        {
            get
            {
                return (DateTime.UtcNow - start).TotalMinutes >= duration;
            }
        }

        public Buff()
        {
            start = DateTime.UtcNow;
        }

        public abstract string Description
        {
            get;
        }
        public abstract void Apply(Player player);
    }
}
