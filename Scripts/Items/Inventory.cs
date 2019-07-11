using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantKitty.Scripts.Data
{
    public struct Inventory
    {
        public int maxSlots;
        public List<InventoryItem> slots;

        public Inventory(int maxSlots)
        {
            this.maxSlots = maxSlots;
            slots = new List<InventoryItem>();
        }

        private InventoryItem GetSlot(string itemName, out int index)
        {
            for (int i = 0; i < slots.Count; i++)
                if (slots[i] != null && slots[i].item.name == itemName)
                {
                    index = i;
                    return slots[i];
                }

            index = -1;
            return null;
        }

        public InventoryItem GetSlot(string itemName)
        {
            foreach (InventoryItem slot in slots)
                if (slot != null && slot.item.name == itemName)
                    return slot;
            return null;
        }

        public bool HasEnoughOf(string itemName, int amountNeeded)
        {
            InventoryItem slot = GetSlot(itemName);
            return slot != null && slot.amount >= amountNeeded;
        }
        public void AddItem(Item item, int amount)
        {
            InventoryItem slot = GetSlot(item.name);
            if ( slot == null)
            {
                if ( slots.Count < maxSlots )
                    slots.Add(new InventoryItem(item, amount));
            } else
            {
                slot.amount += amount;
            }
        }
        public void RemoveItem(Item item, int amount)
        {
            int index;
            InventoryItem slot = GetSlot(item.name, out index);
            if ( slot != null )
            {
                slot.amount -= amount;
                if ( slot.amount < 1 )
                {
                    slots.RemoveAt(index);
                }
            }
        }
        public void RemoveItem(string itemName, int amount)
        {
            int index;
            InventoryItem slot = GetSlot(itemName, out index);
            if (slot != null)
            {
                slot.amount -= amount;
                if (slot.amount < 1)
                {
                    slots.RemoveAt(index);
                }
            }
        }
    }

    public class InventoryItem
    {
        public Item item;
        public int amount;

        public InventoryItem(Item item, int amount)
        {
            this.item = item;
            this.amount = amount;
        }
    }
}
