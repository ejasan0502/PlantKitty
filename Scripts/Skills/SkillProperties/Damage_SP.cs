﻿using System;
using System.Collections.Generic;
using System.Text;
using PlantKitty.Scripts.Data;

namespace PlantKitty.Scripts.Skills.SkillProperties
{
    public class Damage_SP : SkillProperty
    {
        public bool percent;
        public DamageType dmgType;
        public float inflict;

        public override string Description()
        {
            string p = percent ? "%" : "";
            return $"Deal {inflict}{p} {dmgType.ToString()} damage.";
        }

        public override void Apply(Character caster, Character target, ref string log)
        {
            float rawDmg = dmgType == DamageType.physical ? caster.currentStats.PATK : caster.currentStats.MATK;

            Random random = new Random();
            if (random.Next(0, 100) <= caster.currentStats.CRIT)
            {
                rawDmg *= caster.currentStats.CRITDMG;
                log += "CRITICAL\n";
            }

            if (percent)
                rawDmg *= inflict;
            else
                rawDmg += inflict;

            float dmg = rawDmg - (dmgType == DamageType.physical ? target.currentStats.PDEF : target.currentStats.MDEF);
            if (dmg < 1f) dmg = 1f;

            target.Hit(dmg);
            log += $"{target.name} takes {dmg} {dmgType.ToString()} damage!";
        }

        public override string ToDataString()
        {
            return $"Damage_SP$percent>{percent}$dmgType>{dmgType.ToString()}$inflict>{inflict}";
        }
    }
}
