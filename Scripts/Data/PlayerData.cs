using Discord.WebSocket;
using Newtonsoft.Json;
using PlantKitty.Scripts.Actions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantKitty.Scripts.Data
{
    public class PlayerData
    {
        private const string playerDataPath = "Resources/PlayerData/";

        private Dictionary<ulong, Player> players;

        private static PlayerData instance;
        public static PlayerData Instance
        {
            get
            {
                if (instance == null)
                    instance = new PlayerData();
                return instance;
            }
        }

        public List<Player> Players
        {
            get
            {
                return players.Values.ToList();
            }
        }

        private PlayerData()  
        {
            players = new Dictionary<ulong, Player>();
        }

        public Player GetPlayer(ulong id)
        {
            if (players.ContainsKey(id))
                return players[id];
            return null;
        }
        public bool Exists(ulong id)
        {
            return players.ContainsKey(id);
        }

        public void AddPlayer(ulong id, string name)
        {
            if (!players.ContainsKey(id))
            {
                Player p = new Player();
                p.id = id;
                players.Add(id, p);
                SaveData();
            }

            players[id].name = name;
        }
        public void SaveData()
        {
            foreach (ulong key in players.Keys)
            {
                string json = JsonConvert.SerializeObject(players[key]);
                File.WriteAllText(playerDataPath + key + ".json", json);
            }
        }
        public void SavePlayer(ulong id)
        {
            if (players.ContainsKey(id))
            {
                string json = JsonConvert.SerializeObject(players[id]);
                File.WriteAllText(playerDataPath + id + ".json", json);
            }
        }
        public void LoadData(IReadOnlyCollection<SocketUser> users)
        {
            string[] filePaths = Directory.GetFiles(playerDataPath);
            foreach (string path in filePaths)
            {
                string json = File.ReadAllText(path);
                Player p = JsonConvert.DeserializeObject<Player>(json);

                if (!players.ContainsKey(p.id))
                {
                    players.Add(p.id, p);

                    if (p.task is Battling)
                        p.SetTask(null);
                }
            }
            Console.WriteLine("Player data loaded!");

            foreach (SocketUser user in users)
            {
                if (!players.ContainsKey(user.Id))
                {
                    Player p = new Player();
                    p.id = user.Id;
                    p.name = user.Username;
                    players.Add(p.id, p);

                    string json = JsonConvert.SerializeObject(p);
                    File.WriteAllText(playerDataPath + p.id + ".json", json);

                    Console.WriteLine($"Created {p.id} player data file!");
                }
                else
                {
                    players[user.Id].name = user.Username;
                }
            }
        }
    }
}
