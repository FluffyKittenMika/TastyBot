using Discord;
using Discord.Commands;
using Discord.WebSocket;

using DiscordUI.Utility;

using System;
using System.Reflection;
using System.Threading.Tasks;
using Interfaces.Contracts.BusinessLogicLayer;
using Interfaces.Entities.Models;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using Enums.UserPermissions;
using Utilities.LoggingService;
using Utilities.TasksUtilities;
using Interfaces.Entities.ViewModels;
using System.Linq;
using Utilities.Converters;

namespace DiscordUI.Services
{
    public class StartupService
    {
        private readonly IServiceProvider _provider;
        private readonly IUserService _serv;
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;
        private readonly Config _config;

        public StartupService(IServiceProvider provider, IUserService serv, DiscordSocketClient discord, CommandService commands, Config config)
        {
            _provider = provider;
            _serv = serv;
            _config = config;
            _discord = discord;
            _commands = commands;

            
            Logging.LogReadyMessage(this);
        }

        public async Task StartAsync()
        {
            string discordToken = _config.DiscordToken;                                     // Get the discord token from the config file
            if (string.IsNullOrWhiteSpace(discordToken))
            {
                string logMessage = "Please enter your bot's token into the `config.json` file found in the applications root directory.";
                Logging.LogCriticalMessage(GetType().Name, logMessage).PerformAsyncTaskWithoutAwait();
                throw new Exception();
            }

            LoadStaffInDatabase();

            await _discord.LoginAsync(TokenType.Bot, discordToken);                         // Login to discord
            await _discord.StartAsync();                                                    // Connect to the websocket

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);        // Load commands and modules into the command service
        }

        private void LoadStaffInDatabase()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Loading in staff...");
            try
            {
                Console.WriteLine($"{DateTime.UtcNow:hh:mm:ss} [StartUp - Info] Default staff:");
                List<UserCreateVM> staffToUpload = JsonConvert.DeserializeObject<List<UserCreateVM>>(File.ReadAllText(AppContext.BaseDirectory + "staff.json"));

                int staffMemberCount = 0;
                int newlyAddedStaffMember = 0;
                foreach(var staffMember in staffToUpload)
                {
                    if (UploadStaffMember(staffMember))
                    {
                        newlyAddedStaffMember++;
                    }
                    LogStaffMember(staffMember, staffMemberCount);
                    staffMemberCount++;

                }
                
                Console.WriteLine($"{DateTime.UtcNow:hh:mm:ss} [StartUp - Info] Staff successfully loaded in (New loaded: { newlyAddedStaffMember })");
            }
            catch (Exception)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"{DateTime.UtcNow:hh:mm:ss} [StartUp - Critical] No staff file found, please create one (staff.json).");
            }
        }

        private bool UploadStaffMember(UserCreateVM staffMember)
        {
            if (_serv.ByDiscordId(staffMember.DiscordId) == null)
            {
                List<Permissions> permissions = staffMember.Permissions.Select(x => Enum.Parse<Permissions>(x)).ToList();
                User staffMemberCreate = new User()
                {
                    Name = staffMember.Name,
                    DiscordId = staffMember.DiscordId,
                    Administrator = staffMember.Administrator,
                    Permissions = permissions
                };

                _serv.Create(staffMemberCreate);
                return true;
            }
            return false;
        }

        private void LogStaffMember(UserCreateVM staffMember, int staffMemberCount)
        {
            Console.Write($"Staff#{staffMemberCount:D2}: - ");
            foreach (var staffMemberProperty in staffMember.GetType().GetProperties())
            {
                if (staffMemberProperty.PropertyType != typeof(List<string>))
                {
                    Console.Write($"{ staffMemberProperty.Name }: { staffMemberProperty.GetValue(staffMember) } - ");
                }
                else
                {
                    List<string> permissionsStringList = (List<string>)staffMemberProperty.GetValue(staffMember);
                    Console.Write($"{ staffMemberProperty.Name }: { string.Join(", ", permissionsStringList) } - ");
                }
            }
            Console.WriteLine("");
        }
    }
}