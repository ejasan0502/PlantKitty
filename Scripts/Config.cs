using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PlantKitty
{
    class Config
    {
        private const string configFolder = "Resources";
        private const string configFile = "config.json";

        public static BotConfig bot;

        static Config()
        {
            if ( !Directory.Exists(configFolder))
            {
                Directory.CreateDirectory(configFolder);
            }

            string path = configFolder + "/" + configFile;
            if (File.Exists(path))
            {
                bot = JsonConvert.DeserializeObject<BotConfig>(File.ReadAllText(path));
            } else
            {
                bot = new BotConfig("!");
            }
        }
    }

    public struct BotConfig
    {
        public string token;
        public string debugToken;
        public string cmdPrefix;
        public string version;
        public ulong channel;
        public ulong debugChannel;
        public List<string> changes;

        public BotConfig(string prefix)
        {
            token = "";
            debugToken = "";
            cmdPrefix = prefix;
            version = "";
            channel = 0;
            debugChannel = 0;
            changes = new List<string>()
            {
                "Init"
            };
        }
    }
}
