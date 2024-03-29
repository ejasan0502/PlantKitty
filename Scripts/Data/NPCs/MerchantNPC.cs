﻿using Newtonsoft.Json;
using PlantKitty.Scripts.Data.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlantKitty.Scripts.Data
{
    public class MerchantNPC : NPC
    {
        [JsonConverter(typeof(ListStringJsonConverter))] public List<string> sellItems;

        public override string OnInteractDialogue()
        {
            return $"{name}: How can I help you?\n!shop\n!buy \"(Item Name)\" (Amount)\n!sell \"(Item Name)\" (Amount)";
        }
    }
}
