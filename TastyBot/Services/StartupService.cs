using Discord;
using Discord.Commands;
using Discord.WebSocket;

using TastyBot.Utility;
using Interfaces.Contracts.Utilities;

using System;
using System.Reflection;
using System.Threading.Tasks;
using Interfaces.Contracts.BusinessLogicLayer;
using Interfaces.Entities.Models;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using Enums.UserPermissions;

namespace TastyBot.Services
{
    public class StartupService
    {
        private readonly IServiceProvider _provider;
        private readonly IUserRepository _repo;
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;
        private readonly Config _config;
        private readonly ILoggingService _log;

        public StartupService(IServiceProvider provider, IUserRepository repo, ILoggingService log, DiscordSocketClient discord, CommandService commands, Config config)
        {
            _provider = provider;
            _repo = repo;
            _config = config;
            _discord = discord;
            _commands = commands;
            _log = log;

            _log.LogReadyMessage(this);
        }

        public async Task StartAsync()
        {
            string discordToken = _config.DiscordToken;                                     // Get the discord token from the config file
            if (string.IsNullOrWhiteSpace(discordToken))
            {
                string logMessage = "Please enter your bot's token into the `config.json` file found in the applications root directory.";
                await _log.LogAsync(new LogMessage(LogSeverity.Critical, GetType().Name, logMessage));
                throw new Exception();
            }

            LoadStaff();

            await _discord.LoginAsync(TokenType.Bot, discordToken);                         // Login to discord
            await _discord.StartAsync();                                                    // Connect to the websocket

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);        // Load commands and modules into the command service
        }

        private void LoadStaff()
        {
            string time = DateTime.UtcNow.ToString("hh:mm:ss");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Loading in staff...");
            try
            {
                Console.WriteLine($"{time} [StartUp - Info] Default staff:");
                List<UserCreate> staff = JsonConvert.DeserializeObject<List<UserCreate>>(File.ReadAllText(AppContext.BaseDirectory + "staff.json"));
                int staffCount = 0;
                foreach (UserCreate staffMember in staff)
                {
                    if (_repo.ByDiscordId(staffMember.DiscordId) == null)
                        _repo.Create(staffMember);

                    staffCount++;
                    Console.Write($"Staff#{staffCount:D2}: - ");
                    foreach (var staffMemberProperty in staffMember.GetType().GetProperties())
                    {
                        if(staffMemberProperty.PropertyType != typeof(List<Permissions>))
                        {
                            Console.Write($"{staffMemberProperty.Name}: {staffMemberProperty.GetValue(staffMember)} - ");
                        }
                    }
                    Console.WriteLine("");
                }
                time = DateTime.UtcNow.ToString("hh:mm:ss");
                Console.WriteLine($"{time} [StartUp - Info] Staff successfully loaded in");
            }
            catch (Exception)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"{time} [StartUp - Critical] No staff file found, please create one, or the bot simply will not work.");
            }
        }
    }
}