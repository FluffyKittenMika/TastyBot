using TastyBot.Utility;
using Utilities.RainbowUtilities;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using System;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using HeadpatPictures.Contracts;
using Utilities.LoggingService;

namespace TastyBot.Services
{
    public class CommandHandlingService
    {
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _discord;
        private readonly Config _config;
        private readonly IServiceProvider _services;
        private readonly ICatModule _catModule;

        public CommandHandlingService(DiscordSocketClient discord, CommandService commands, Config config, IServiceProvider services, ICatModule catModule)
        {
            _discord = discord;
            _commands = commands;
            _config = config;
            _services = services;
            _catModule = catModule;

            //Hook Messages so we can process them if they're commands.
            _discord.MessageReceived += OnMessageReceivedAsync;
            _commands.CommandExecuted += OnCommandExecuted;
            _discord.Log += Logging.LogAsync;
            _commands.Log += Logging.LogAsync;

            Logging.LogReadyMessage(this);
        }

        private async Task OnCommandExecuted(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            if (!command.IsSpecified) //if the command does not exist, truncate response
                return;

            if (!result.IsSuccess) // If not successful, reply with the error.
                await context.Channel.SendMessageAsync(result.ToString());
        }

        private async Task OnMessageReceivedAsync(SocketMessage socketMessage)
        {
            if (!(socketMessage is SocketUserMessage userMessage)) return;      // Ensure the message is from a user/bot
            if (userMessage.Author.Id == _discord.CurrentUser.Id) return;       // Ignore self when checking commands

            await ExecuteCommand(userMessage);
            await RainbowChannelChangeColor(socketMessage);
            await CatifyMessage(userMessage);
        }

        private async Task ExecuteCommand(SocketUserMessage userMessage)
        {
            var context = new SocketCommandContext(_discord, userMessage);      // Create the command context

            int argPos = 0;                                             // Check if the message has a valid command prefix
            if (userMessage.HasStringPrefix(_config.Prefix, ref argPos) || userMessage.HasMentionPrefix(_discord.CurrentUser, ref argPos))
            {
                var result = await _commands.ExecuteAsync(context, argPos, _services);      // Execute the command
                if (!result.IsSuccess)
                {
                    await Logging.LogAsync(new LogMessage(LogSeverity.Error, GetType().Name, result.ErrorReason));
                }
            }
        }

        private async Task RainbowChannelChangeColor(SocketMessage socketMessage)
        {
            // Ignore system messages, or messages from other bots
            if (!(socketMessage is SocketUserMessage message)) return;
            if (message.Source != MessageSource.User) return;

            //The try is here to ignore anything that we don't give a shit about :)
            try
            {
                foreach (IRole role in ((IGuildChannel)socketMessage.Channel).Guild.Roles)
                {
                    if (role.Name.ToLower() == "team rainbow")
                    {
                        await role.ModifyAsync(x =>
                        {
                            x.Color = RainbowUtilities.CreateRainbowColor();
                        });
                    }
                }
            }
            catch (Exception e)
            {
                await Logging.LogAsync(new LogMessage(LogSeverity.Error, GetType().Name, "Unable to change rainbow channel name; ", e));
            }
        }

        public async Task CatifyMessage(SocketUserMessage userMessage)
        {
            //Catbot channel
            if (userMessage.Channel.Name.ToLower() == "botcat") //More generic
            {
                string logMessage = $"catbot:{userMessage.Author} - {userMessage.Content.ToLower()} - cattified :3";
                await Logging.LogDebugMessage(GetType().Name, logMessage);

                //remove the evicence
                await userMessage.DeleteAsync();

                if (userMessage.Content.Length > 0 && userMessage.Content != null)
                {
                    string content = userMessage.Content;
                    // Replace invalid characters with empty strings.
                    try
                    {
                        content = Regex.Replace(content, @"[^a-zA-Z0-9 ]+", "", RegexOptions.None, TimeSpan.FromSeconds(5));
                        logMessage = $"Regexed into: {content}";
                        await Logging.LogDebugMessage(GetType().Name, logMessage);
                    }
                    catch (TimeoutException)
                    {
                        //let the masses know that home hosting is not always so great, but it beats spending money on an dedicated machine, but i'm seriously considering making a community hosted server lol.
                        await userMessage.Channel.SendMessageAsync("Can't get a cat atm, we're experiencing connection issues and we're timing out. Tastybot crew is sorry for this atrocity. And we will punish the ISP gods promptly. Also @Mikaelssen if nothing happens.");
                    }
                    // Get a stream containing an image of a cat
                    if (content == "" || content.Length == 0)
                        content = "error :)";
                    var stream = await _catModule.CatPictureAsync(32, "", content);
                    await userMessage.Channel.SendFileAsync(stream, "cat.png");
                }
            }
        }
    }
}