﻿using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Discord;
using PlantKitty.Scripts.Data;
using PlantKitty.Scripts.Combat;
using Newtonsoft.Json;
using PlantKitty.Scripts.Data.Converters;

namespace PlantKitty
{
    class Program
    {
        static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();

        private const bool debugging = true;

        private DiscordSocketClient client;
        private CommandService commands;
        private IServiceProvider service;
        private GameData gameData;
        private PlayerData playerData;
        private BattleManager battleManager;

        public async Task RunBotAsync()
        {
            JsonConvert.DefaultSettings = () =>
            {
                var settings = new JsonSerializerSettings();
                settings.TypeNameHandling = TypeNameHandling.All;
                settings.Formatting = Formatting.Indented;
                settings.Converters.Add(new StatsJsonConverter());
                settings.Converters.Add(new AttributesJsonConverter());
                settings.Converters.Add(new ConsumePropertyJsonConverter());
                return settings;
            };

            client = new DiscordSocketClient();
            commands = new CommandService();
            gameData = GameData.Instance;
            playerData = PlayerData.Instance;
            battleManager = BattleManager.Instance;

            service = new ServiceCollection()
                .AddSingleton(client)
                .AddSingleton(commands)
                .BuildServiceProvider();

            client.Log += Log;

            await RegisterCommandsAsync();
            await client.LoginAsync(Discord.TokenType.Bot, !debugging ? Config.bot.token : Config.bot.debugToken);
            await client.StartAsync();

            client.Ready += OnReady;

            await Task.Delay(-1);
        }

        private async Task OnReady()
        {
            var channel = client.GetChannel(!debugging ? Config.bot.channel : Config.bot.debugChannel);
            playerData.LoadData(channel.Users);

            if (!debugging)
            {
                string startupText = $"Version: {Config.bot.version}\n";
                for (int i = 0; i < Config.bot.changes.Count; i++)
                {
                    if (i != 0) startupText += "\n";
                    startupText += Config.bot.changes[i];
                }
                await ((IMessageChannel)channel).SendMessageAsync(startupText);
            }
            else
            {
                await ((IMessageChannel)channel).SendMessageAsync("Bot is now running!");
            }

            await Task.Delay(1);
        }
        private Task Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }
        private async Task RegisterCommandsAsync()
        {
            client.MessageReceived += HandleCommandAsync;
            await commands.AddModulesAsync(Assembly.GetEntryAssembly(), service);
        }
        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;

            if (message is null || message.Author.IsBot) return; 

            int argPos = 0;
            if ( message.HasStringPrefix(Config.bot.cmdPrefix, ref argPos) || message.HasMentionPrefix(client.CurrentUser, ref argPos))
            {
                var context = new SocketCommandContext(client, message);

                var result = await commands.ExecuteAsync(context, argPos, service);

                if (!result.IsSuccess)
                    Console.WriteLine(result.ToString());
            }
        }
    }
}
