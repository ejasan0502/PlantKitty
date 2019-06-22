using Newtonsoft.Json;
using PlantKitty.Scripts.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlantKitty.Scripts.Statuses
{
    public abstract class StatusProperty
    {
        public abstract string Description(Character caster);
        public abstract void Apply(Character caster, Character target);
        public abstract string ToDataString();
    }
}
