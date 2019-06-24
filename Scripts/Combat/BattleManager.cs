using Discord;
using PlantKitty.Scripts.Actions;
using PlantKitty.Scripts.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PlantKitty.Scripts.Combat
{
    public class BattleManager
    {
        private const int maxMonsterCount = 6;
        private const int maxLootPerMonster = 3;
        private Dictionary<string, Battle> battles;

        private static BattleManager instance;
        public static BattleManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new BattleManager();
                return instance;
            }
        }

        public BattleManager()
        {
            battles = new Dictionary<string, Battle>();
        }

        private List<Monster> GenerateMonsters(int level, string fieldName)
        {
            Field field = GameData.Instance.GetField(fieldName);
            if (field == null) return null;
            if (field.monsters == null) return null;

            Random random = new Random();
            int max = 1;
            if (level >= 20) max = maxMonsterCount;
            else if (level >= 10) max = 4;
            else if (level >= 5) max = 2;
            int maxCount = random.Next(1, max);

            List<Monster> monsters = new List<Monster>();
            for (int i = 0; i < maxCount; i++)
            {
                string monsterName = field.monsters[random.Next(0, field.monsters.Count)];
                Monster monster = GameData.Instance.GetMonster(monsterName);
                if (monster == null) continue;
                monsters.Add(monster);
            }

            return monsters;
        }
        private string GenerateUniqueID()
        {
            string uniqueId = Guid.NewGuid().ToString();
            return uniqueId;
        }

        private async Task DisplayCharacters(string title, List<Monster> characters, IMessageChannel channel)
        {
            EmbedBuilder builder = new EmbedBuilder()
                .WithTitle(title);
            for (int i = 0; i < characters.Count; i++)
            {
                string info = $"HP: {characters[i].currentStats.HP}/{characters[i].maxStats.HP}" +
                              $"\nMP: {characters[i].currentStats.MP}/{characters[i].maxStats.MP}";
                builder.AddField($"{i+1}. {characters[i].name}", info, true);
            }

            await channel.SendMessageAsync(null, false, builder.Build());
        }
        private async Task DisplayCharacters(string title, List<Player> characters, IMessageChannel channel)
        {
            EmbedBuilder builder = new EmbedBuilder()
                .WithTitle(title);
            for (int i = 0; i < characters.Count; i++)
            {
                string info = $"HP: {characters[i].currentStats.HP}/{characters[i].maxStats.HP}" +
                              $"\nMP: {characters[i].currentStats.MP}/{characters[i].maxStats.MP}";
                builder.AddField($"{i+1}. {characters[i].name}", info, true);
            }

            await channel.SendMessageAsync(null, false, builder.Build());
        }
        private CombatAction CreateRandomAction(Monster monster, List<Player> players)
        {
            Random random = new Random();
            return new Attack(monster, players[random.Next(0,players.Count)]);
        }
        private async Task Victory(Battle battle, IMessageChannel channel)
        {
            await channel.SendMessageAsync($"Victory!\nGenerate rewards...");

            // Calculate experience gained
            float xpGain = 0f;
            foreach (Monster m in battle.monsters)
                xpGain += m.exp;
            xpGain += xpGain * 0.05f * (battle.players.Count - 1);

            // Give experience and loot to players
            foreach (Player p in battle.players)
            {

                // Add XP
                p.AddExp(xpGain);
                await channel.SendMessageAsync($"You have gained {xpGain} experience!");

                // Check level up
                if (p.CheckIfLeveledUp(out Attributes changes))
                {
                    await channel.SendMessageAsync("You have leveled up!");

                    FieldInfo[] fields = changes.GetType().GetFields();
                    foreach (FieldInfo f in fields)
                    {
                        int val = (int)f.GetValue(changes);
                        if (val != 0)
                            await channel.SendMessageAsync($"{f.Name}: {UtilityMethods.ToSignString(val)}");
                    }
                }

                // Generate loot
                Dictionary<string, InventoryItem> loot = new Dictionary<string, InventoryItem>();
                foreach (Monster m in battle.monsters)
                {
                    Random random = new Random();
                    int lootCount = random.Next(1, maxLootPerMonster);
                    for (int i = 0; i < lootCount; i++)
                    {
                        Item item = GameData.Instance.GetRandomItem(m.loot, null, p.attributes.LUK);
                        if (item != null)
                        {
                            if (!loot.ContainsKey(item.name))
                                loot.Add(item.name, new InventoryItem(item, 0));
                            loot[item.name].amount += random.Next(1, (int)item.tier);
                        }
                    }
                }

                // Add loot to inventory
                foreach (KeyValuePair<string, InventoryItem> l in loot)
                {
                    p.inventory.AddItem(l.Value.item, l.Value.amount);
                    await channel.SendMessageAsync($"Gained x{l.Value.amount} {l.Key}");
                }

                p.SetTask(null);
                PlayerData.Instance.SavePlayer(p.id);
            }

            RemoveBattle(battle.id);
        }
        private async Task Defeat(Battle battle, IMessageChannel channel)
        {
            await channel.SendMessageAsync("Defeat!");
            foreach (Player p in battle.players)
            {
                float xpLoss = p.GetMaxExp() * 0.1f;
                p.AddExp(-xpLoss);
                await channel.SendMessageAsync($"{p.name} have lost {xpLoss} experience!");

                p.SetTask(null);
                PlayerData.Instance.SavePlayer(p.id);
            }

            battles.Remove(battle.id);
        }

        public Battle GetBattle(string id)
        {
            if (battles.ContainsKey(id))
                return battles[id];
            Console.WriteLine($"Cannot find battle id, {id}");
            return null;
        }

        public async Task RegisterBattle(Player player, IMessageChannel channel)
        {
            Battle battle = new Battle()
            {
                id = GenerateUniqueID(),
                players = new List<Player>() { player },
                monsters = GenerateMonsters(player.level, player.field)
            };

            if (battle.monsters != null && battle.monsters.Count > 0 && !battles.ContainsKey(battle.id))
            {
                battles.Add(battle.id, battle);

                Battling task = new Battling(battle.id);
                player.SetTask(task);
                await channel.SendMessageAsync($"You are now in combat!");
                await DisplayCharacters("Monsters", battle.monsters, channel);
            }
            else
                await channel.SendMessageAsync($"You found no monsters to fight...");
        }
        public async Task PerformBattle(Battle battle, IMessageChannel channel)
        {
            List<CombatAction> actions = battle.GetActions();
            if (actions.Count >= battle.players.Count)
            {
                await channel.SendMessageAsync("Starting battle...");

                // Generate actions for each monster
                foreach (Monster monster in battle.monsters)
                    actions.Add(CreateRandomAction(monster, battle.players));

                // Sort actions based on speed
                actions.Sort((x, y) => y.GetSelf().currentStats.SPD.CompareTo(x.GetSelf().currentStats.SPD));

                // Perform actions
                foreach (CombatAction action in actions)
                    await action.Perform(channel);

                // Reset
                battle.ClearActions();

                // Apply status
                foreach (Monster m in battle.monsters)
                    m.CheckStatuses();
                foreach (Player p in battle.players)
                    p.CheckStatuses();

                // Display update players info
                await DisplayCharacters("Players", battle.players, channel);

                // Display updated monsters info
                await DisplayCharacters("Monsters", battle.monsters, channel);

                // Check players
                List<Player> alivePlayers = battle.players.Where(p => p.currentStats.HP > 0).ToList();
                if (alivePlayers.Count < 1)
                {
                    await Defeat(battle, channel);
                }
                // Check monsters
                List<Monster> aliveMonsters = battle.monsters.Where(m => m.currentStats.HP > 0).ToList();
                if (aliveMonsters.Count < 1)
                {
                    await Victory(battle, channel);
                }
            }
            else
                await channel.SendMessageAsync("Waiting on other players...");
        }
        public void RemoveBattle(string battleId)
        {
            if (battles.ContainsKey(battleId))
                battles.Remove(battleId);
        }
    }
}
