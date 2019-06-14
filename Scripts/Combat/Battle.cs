using PlantKitty.Scripts.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantKitty.Scripts.Combat
{
    public class Battle
    {
        public string id;
        public List<Player> players;
        public List<Monster> monsters;

        private List<CombatAction> actions;

        public Battle()
        {
            players = new List<Player>();
            monsters = new List<Monster>();
            actions = new List<CombatAction>();
        }

        private bool CheckForDuplicate(CombatAction action)
        {
            foreach (CombatAction a in actions)
            {
                if (a.GetSelf() == action.GetSelf())
                    return true;
            }
            return false;
        }

        public bool HasPlayer(ulong id)
        {
            foreach (Player p in players)
                if (p.id == id)
                    return true;
            return false;
        }
        public List<CombatAction> GetActions()
        {
            return actions;
        }

        public void AddAction(CombatAction action)
        {
            if (!CheckForDuplicate(action))
            {
                actions.Add(action);
            }
        }
        public void ClearActions()
        {
            actions = new List<CombatAction>();
        }
    }
}
