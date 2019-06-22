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
                return stats.ToStringWithSigns(percent);
            }
        }
        public override void Apply(Player player)
        {
            player.maxStats += stats;
        }
    }
}
