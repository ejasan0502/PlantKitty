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
    // Note: When adding a new command, remember to add to CommandCategory enum in Command script

    public class NPCCommands : PlayerCommand
    {
        [Command("talk")]
        public async Task Talk(string npcName)
        {
            Player player;
            string log;
            if (CheckPlayer(out player, out log))
            {
                Field field = GameData.Instance.GetField(player.field);
                if (field == null) return;

                if (field.HasNPC(npcName))
                {
                    NPC npc = GameData.Instance.GetNPC(npcName);
                    if (npc != null)
                        log = npc.OnInteractDialogue();
                }
                else
                    log = $"{Context.User.Mention}. Unable to find {npcName} at current location!";
            }

            if (log != "")
                await ReplyAsync(log);
        }
        [Command("shop")]
        public async Task Shop(string npcName)
        {
            Player player;
            string log;
            if (CheckPlayer(out player, out log))
            {
                Field field = GameData.Instance.GetField(player.field);
                if (field == null) return;

                if (field.HasNPC(npcName))
                {
                    NPC npc = GameData.Instance.GetNPC(npcName);
                    if (npc != null && npc is MerchantNPC)
                    {
                        log = $"{npc.name}: Here's what I have in stock.";
                        foreach (string itemName in ((MerchantNPC)npc).sellItems)
                        {
                            Item item = GameData.Instance.GetItem(itemName);
                            if (item == null) continue;
                            log += $"\n{itemName} ({item.buyValue})";
                        }
                    }
                    else
                        log = $"{Context.User.Mention}. {npcName} is not a merchant NPC!";
                }
                else
                    log = $"{Context.User.Mention}. Unable to find {npcName} at current location!";
            }

            if (log != "")
                await ReplyAsync(log);
        }
        [Command("buy")]
        public async Task Buy(string npcName, string itemName, int amount)
        {
            Player player;
            string log;
            if (CheckPlayer(out player, out log))
            {
                Field field = GameData.Instance.GetField(player.field);
                if (field == null) return;

                if (field.HasNPC(npcName))
                {
                    NPC npc = GameData.Instance.GetNPC(npcName);
                    if (npc != null && npc is MerchantNPC)
                    {
                        Item item = GameData.Instance.GetItem(itemName);
                        if (item == null) return;

                        InventoryItem inventoryItem = player.inventory.GetSlot(itemName);
                        if (inventoryItem != null && inventoryItem.amount >= amount * item.buyValue)
                        {
                            player.inventory.AddItem(item, amount);
                            player.inventory.RemoveItem(inventoryItem.item, amount * item.buyValue);
                            log = $"{Context.User.Mention}. You have purchased {amount} {itemName} for {amount * item.buyValue} copper!";
                        }
                        else
                            log = $"{Context.User.Mention}. You do not have enough copper coins...";
                    }
                    else
                        log = $"{Context.User.Mention}. {npcName} is not a merchant NPC!";
                }
                else
                    log = $"{Context.User.Mention}. Unable to find {npcName} at current location!";
            }

            if (log != "")
                await ReplyAsync(log);
        }
        [Command("sell")]
        public async Task Sell(string npcName, string itemName, int amount)
        {
            Player player;
            string log;
            if (CheckPlayer(out player, out log))
            {
                Field field = GameData.Instance.GetField(player.field);
                if (field == null) return;

                if (field.HasNPC(npcName))
                {
                    NPC npc = GameData.Instance.GetNPC(npcName);
                    if (npc != null && npc is MerchantNPC)
                    {
                        if (player.inventory.HasEnoughOf(itemName, amount))
                        {
                            Item item = GameData.Instance.GetItem("Copper Coin");
                            if (item == null) return;

                            Item itemToSell = GameData.Instance.GetItem(itemName);
                            if (itemToSell == null) return;

                            player.inventory.AddItem(item, amount * itemToSell.sellValue);
                            player.inventory.RemoveItem(itemToSell, amount);
                        }
                        else
                            log = $"{Context.User.Mention}. You don't have enough {itemName} to sell!";
                    }
                    else
                        log = $"{Context.User.Mention}. {npcName} is not a merchant NPC!";
                }
                else
                    log = $"{Context.User.Mention}. Unable to find {npcName} at current location!";
            }

            if (log != "")
                await ReplyAsync(log);
        }
    }
}
