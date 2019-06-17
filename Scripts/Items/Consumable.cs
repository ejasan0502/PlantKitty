using Newtonsoft.Json;
using PlantKitty.Scripts.Data.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantKitty.Scripts.Data
{
    public class Consumable : Item
    {
        public bool friendly;
        [JsonConverter(typeof(ListConsumePropertyJsonConverter))] public List<ConsumeProperty> properties;

        public override string Description
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < properties.Count; i++)
                {
                    string text = "";
                    if (i != 0) text += "\n";
                    text += properties[i].Description;

                    builder.Append(text);
                }

                return builder.ToString();
            }
        }

        public Consumable(string name, bool friendly, params ConsumeProperty[] propertyList) : base(name)
        {
            this.friendly = friendly;
            if (propertyList != null)
                properties = propertyList.ToList();
            else
                properties = new List<ConsumeProperty>();
        }

        public void Use(Inventory inventory, Character target)
        {
            foreach (ConsumeProperty property in properties)
            {
                property.Apply(target);
            }
            inventory.RemoveItem(this, 1);
        }
    }
}
