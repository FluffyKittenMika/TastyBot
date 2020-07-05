﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;

using TastyBot.Contracts;
using TastyBot.Utility;

using System;
using System.Reflection;
using System.Threading.Tasks;

namespace TastyBot.Services
{
    public class StartupService : IStartupService
    {
        private readonly IServiceProvider _provider;
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;
        private readonly Config _config;

        private readonly ILoggingService _log;
        private readonly string _logSource;

        public StartupService(IServiceProvider provider, ILoggingService log, DiscordSocketClient discord, CommandService commands, Config config)
        {
            _provider = provider;
            _config = config;
            _discord = discord;
            _commands = commands;

            _log = log;
            _logSource = typeof(StartupService).Name;
        }

        public async Task StartAsync()
        {
            string discordToken = _config.DiscordToken;                                     // Get the discord token from the config file
            if (string.IsNullOrWhiteSpace(discordToken))
            {
                string logMessage = "Please enter your bot's token into the `config.json` file found in the applications root directory.";
                await _log.LogAsync(new LogMessage(LogSeverity.Critical, _logSource, logMessage));
                throw new Exception();
            }
                

            await _discord.LoginAsync(TokenType.Bot, discordToken);                         // Login to discord
            await _discord.StartAsync();                                                    // Connect to the websocket

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);        // Load commands and modules into the command service
        }
    }
}
