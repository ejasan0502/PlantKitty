using Discord;
using Discord.Commands;
using Discord.WebSocket;
using PlantKitty.Scripts.Actions;
using PlantKitty.Scripts.Combat;
using PlantKitty.Scripts.Data;
using PlantKitty.Scripts.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantKitty.Commands
{
    public class BattleCommands : PlayerCommand
    {
        private bool IsPrivate(IMessageChannel channel, out string log)
        {
            if (channel is SocketDMChannel)
            {
                log = "";
                return true;
            } else
            {
                log = $"{Context.User.Mention}. You can only send battle commands in a private channel!";
                return false;
            }
        }
        private bool IsValidIndex(int index, List<Character> characters, out string log)
        {
            log = $"{Context.User.Mention}. Invalid index...";
            if (index < 0 || index >= characters.Count) return false;
            if (characters[index].currentStats.HP <= 0) return false;

            log = "";
            return true;
        }

        [Command("battle")]
        public async Task StartBattle()
        {
            Player player;
            string log;

            if (!IsPrivate(Context.Channel, out log))
            {
                await ReplyAsync(log);
                return;
            }

            if (CheckIfBusy(out player, out log))
            {
                await BattleManager.Instance.RegisterBattle(player, Context.Channel);
            } else 
                await ReplyAsync(log);
        }
        [Command("attack", RunMode = RunMode.Async)]
        public async Task Attack(int index = 0)
        {
            Player player;
            string log;

            if (IsPrivate(Context.Channel, out log))
            {
                if (CheckPlayer(out player, out log) && IsInBattle(player))
                {
                    Battling task = player.task as Battling;
                    Battle battle = BattleManager.Instance.GetBattle(task.battleId);

                    if (index == 0) index++;
                    if (IsValidIndex(index-1, battle.monsters.Cast<Character>().ToList(), out log))
                    {
                        Attack action = new Attack(player, battle.monsters[index-1]);
                        battle.AddAction(action);

                        await BattleManager.Instance.PerformBattle(battle, Context.Channel);
                    }
                }
            }

            if (log != "")
                await ReplyAsync(log);
        }
        [Command("use", RunMode = RunMode.Async)]
        public async Task Use(string itemName, int index)
        {
            Player player;
            string log;

            if (IsPrivate(Context.Channel, out log))
            {
                if (CheckPlayer(out player, out log) && IsInBattle(player))
                {
                    Battling task = player.task as Battling;
                    Battle battle = BattleManager.Instance.GetBattle(task.battleId);

                    Item item = GameData.Instance.GetItem(itemName);
                    if (item != null && item is Consumable)
                    {
                        Consumable consumable = item as Consumable;
                        List<Character> characters;
                        if (consumable.friendly)
                            characters = battle.players.Cast<Character>().ToList();
                        else
                            characters = battle.monsters.Cast<Character>().ToList();

                        if (IsValidIndex(index - 1, characters, out log))
                        {
                            Use action = new Use(consumable, player, characters[index - 1]);
                            battle.AddAction(action);

                            await BattleManager.Instance.PerformBattle(battle, Context.Channel);
                        }
                    }
                    else
                        log = $"{Context.User.Mention}. Unknown consumable item...";
                }
            }

            if (log != "")
                await ReplyAsync(log);
        }
        [Command("flee", RunMode = RunMode.Async)]
        public async Task Flee()
        {
            Player player;
            string log;

            if (!IsPrivate(Context.Channel, out log))
            {
                await ReplyAsync(log);
                return;
            }

            if (CheckPlayer(out player, out log) && IsInBattle(player))
            {
                Battling task = player.task as Battling;
                Battle battle = BattleManager.Instance.GetBattle(task.battleId);

                Random random = new Random();
                if (random.Next(0,100) <= 50 + player.currentStats.EVA)
                {
                    await ReplyAsync($"{Context.User.Mention}. You have successfully fled!");
                    BattleManager.Instance.RemoveBattle(battle.id);
                    player.SetTask(null);

                    battle = null;
                    task = null;
                } else
                {
                    NoAction action = new NoAction(player);
                    battle.AddAction(action);

                    await BattleManager.Instance.PerformBattle(battle, Context.Channel);
                }
            }

            if (log != "")
                await ReplyAsync(log);
        }

        [Command("cast", RunMode = RunMode.Async)]
        public async Task Cast(string skillName, int index)
        {
            Player player;
            string log;

            if (IsPrivate(Context.Channel, out log))
            {
                if (CheckPlayer(out player, out log) && IsInBattle(player))
                {
                    Skill skill = GameData.Instance.GetSkill(skillName);
                    if (skill != null)
                    {
                        if (player.HasSkill(skillName))
                        {
                            Battling task = player.task as Battling;
                            Battle battle = BattleManager.Instance.GetBattle(task.battleId);

                            List<Character> characters = skill.isFriendly ? battle.players.Cast<Character>().ToList() : battle.monsters.Cast<Character>().ToList();
                            if (IsValidIndex(index - 1, characters, out log))
                            {
                                Cast action;
                                if (skill.isAoe)
                                    action = new Cast(player, characters, skill);
                                else
                                    action = new Cast(player, characters[index - 1], skill);
                                battle.AddAction(action);

                                await BattleManager.Instance.PerformBattle(battle, Context.Channel);
                            }
                        }
                        else
                            log = $"{Context.User.Mention}. You have not learned {skillName}!";
                    }
                    else
                        log = $"{Context.User.Mention}. Unknown skill name...";
                }
            }

            if (log != "")
                await ReplyAsync(log);
        }
    }
}
