using Discord;
using Discord.Commands;
using Discord.WebSocket;
using PlantKitty.Scripts.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantKitty.Commands
{
    public class InventoryCommands : PlayerCommand
    {

        [Command("equip")]
        public async Task DisplayEquipsInInventory()
        {
            Player player;
            string log;
            if (CheckPlayer(out player, out log))
            {
                if (IsInBattle(player))
                {
                    log = $"{Context.User.Mention}. This command is not available during combat!";
                } else
                {
                    List<InventoryItem> equips = player.inventory.slots.Where<InventoryItem>(ii => ii.item is Equip).ToList();
                    if (equips.Count > 0)
                    {
                        EmbedBuilder builder = new EmbedBuilder()
                            .WithTitle($"Equipment in @{Context.User.Username}'s Inventory");

                        foreach (InventoryItem ii in equips)
                        {
                            builder.AddField(ii.item.name, ((Equip)ii.item).Description, true);
                        }

                        await ReplyAsync(null, false, builder.Build());
                    }
                    else
                        log = $"{Context.User.Mention}. No equipment found in inventory!";
                }
            }

            if (log != "")
                await ReplyAsync(log);
        }
        [Command("equip")]
        public async Task EquipItem(string itemName)
        {
            Player player;
            string log;
            if (CheckPlayer(out player, out log))
            {
                if (IsInBattle(player))
                {
                    log = $"{Context.User.Mention}. This command is not available during combat!";
                }
                else
                {
                    InventoryItem equipItem = player.inventory.slots
                    .Where<InventoryItem>(ii => ii.item is Equip && ii.item.name.ToLower() == itemName.ToLower()).FirstOrDefault();

                    if (equipItem != null)
                    {
                        player.Equip(equipItem.item as Equip);
                        player.inventory.RemoveItem(equipItem.item, 1);
                        PlayerData.Instance.SavePlayer(player.id);
                        log = $"{Context.User.Mention}. Equipped {itemName}!";
                    }
                    else
                        log = $"{Context.User.Mention}. Unable to find the equippable item, {itemName}, in inventory!";
                }
            }

            if (log != "")
                await ReplyAsync(log);
        }

        [Command("use")]
        public async Task DisplayConsumablesInInventory()
        {
            Player player;
            string log;
            if (CheckPlayer(out player, out log))
            {
                if (IsInBattle(player))
                {
                    log = $"{Context.User.Mention}. This command is not available during combat!";
                }
                else
                {
                    // Display all consumables in inventory
                    List<InventoryItem> items = player.inventory.slots.Where<InventoryItem>(ii => ii.item is Consumable).ToList();
                    if (items.Count > 0)
                    {
                        EmbedBuilder builder = new EmbedBuilder()
                            .WithTitle($"Consumables in @{Context.User.Username}'s Inventory");

                        foreach (InventoryItem ii in items)
                        {
                            builder.AddField(ii.item.name, ((Consumable)ii.item).Description, true);
                        }

                        await ReplyAsync(null, false, builder.Build());
                    }
                }
            }

            if (log != "")
                await ReplyAsync(log);
        }
        [Command("use")]
        public async Task UseItem(string itemName)
        {
            Player player;
            string log;
            if (CheckPlayer(out player, out log))
            {
                if (IsInBattle(player))
                {
                    log = $"{Context.User.Mention}. This command is not available during combat!";
                }
                else
                {
                    InventoryItem inventoryItem = player.inventory.slots
                    .Where<InventoryItem>(ii => ii.item is Consumable && ii.item.name.ToLower() == itemName.ToLower()).FirstOrDefault();

                    if (inventoryItem != null)
                    {
                        Consumable consumeable = inventoryItem.item as Consumable;
                        if (consumeable.friendly)
                        {
                            log = $"{Context.User.Mention}. You used 1 {itemName}!";

                            // Use item
                            ((Consumable)inventoryItem.item).Use(player, ref log);
                            player.inventory.RemoveItem(inventoryItem.item, 1);
                            PlayerData.Instance.SavePlayer(player.id);
                        }
                        else
                            log = $"{Context.User.Mention}. {itemName} cannot be used on you!";
                    }
                    else
                        log = $"{Context.User.Mention}. Cannot find the consumable, {itemName}, in inventory...";
                }
            }
            
            if (log != "")
                await ReplyAsync(log);
        }

        [Command("drop")]
        public async Task DropItem(string itemName, int amount = 0)
        {
            Player player;
            string log;
            if (CheckPlayer(out player, out log))
            {
                InventoryItem inventoryItem = player.inventory.slots
                    .Where<InventoryItem>(ii => ii.item.name.ToLower() == itemName.ToLower()).FirstOrDefault();
                if (inventoryItem != null)
                {
                    if (amount == 0)
                        player.inventory.RemoveItem(inventoryItem.item, inventoryItem.amount);
                    else
                        player.inventory.RemoveItem(inventoryItem.item, amount);
                    PlayerData.Instance.SavePlayer(player.id);
                    log = $"{Context.User.Mention}. You dropped {amount} {itemName}!";
                }
                else
                    log = $"{Context.User.Mention}. Unable to find {itemName} in inventory...";
            }
            
            if (log != "")
                await ReplyAsync(log);
        }
        [Command("inventory")]
        public async Task CheckInventory()
        {
            Player player;
            string log;
            if (CheckPlayer(out player, out log))
            {
                if (player.inventory.slots.Count > 0)
                {
                    EmbedBuilder builder = new EmbedBuilder()
                        .WithTitle($"{Context.User.Username}'s Inventory");

                    int count = 0;
                    foreach (InventoryItem inventoryItem in player.inventory.slots)
                    {
                        builder.AddField($"{inventoryItem.item.name} x{inventoryItem.amount}", inventoryItem.item.Description, count < 4);
                        count++;
                        if (count > 3) count = 0;
                    }
                    await ReplyAsync(null, false, builder.Build());
                }
                else
                    log = $"{Context.User.Mention}. You have no items in inventory!";
            }

            if (log != "")
                await ReplyAsync(log);
        }
    }
}
