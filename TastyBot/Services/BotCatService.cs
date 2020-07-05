using Discord;
using Discord.WebSocket;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HeadpatPictures.Contracts;
using TastyBot.Contracts;

namespace TastyBot.Services
{
    public class BotCatService
    {
        private readonly DiscordSocketClient _discord;
        private readonly ICatModule _module;
        private readonly ILoggingService _log;
        private readonly string _logSource;

        public BotCatService(ILoggingService log, ICatModule module, DiscordSocketClient discord)
        {
            _discord = discord;
            //we'll just change the role colors when we get a message on the discord, instead of doing it over time ;)
            _discord.MessageReceived += OnMessageReceivedAsync;
            _module = module;
            _log = log;
            _logSource = typeof(BotCatService).Name;

            string logMessage = "Ready";
            _log.LogAsync(new LogMessage(LogSeverity.Info, _logSource, logMessage));
        }

        public async Task OnMessageReceivedAsync(SocketMessage arg)
        {
            // Ignore system messages, or messages from other bots
            if (!(arg is SocketUserMessage message)) return;
            if (message.Source != MessageSource.User) return;

            //Catbot channel
            if (arg.Channel.Name.ToLower() == "botcat") //More generic
            {
                string logMessage = $"catbot:{message.Author} - {message.Content.ToLower()} - cattified :3";
                await _log.LogDebugMessage(_logSource, logMessage);

                //remove the evicence
                await arg.DeleteAsync();

                if (arg.Content.Length > 0 && arg.Content != null)
                {
                    string content = arg.Content;
                    // Replace invalid characters with empty strings.
                    try
                    {
                        content = Regex.Replace(content, @"[^a-zA-Z0-9 ]+", "", RegexOptions.None, TimeSpan.FromSeconds(5));
                        logMessage = $"Regexed into: {content}";
                        await _log.LogDebugMessage(_logSource, logMessage);
                    }
                    catch (TimeoutException)
                    {
                        //let the masses know that home hosting is not always so great, but it beats spending money on an dedicated machine, but i'm seriously considering making a community hosted server lol.
                        await arg.Channel.SendMessageAsync("Can't get a cat atm, we're experiencing connection issues and we're timing out. Tastybot crew is sorry for this atrocity. And we will punish the ISP gods promptly. Also @Mikaelssen if nothing happens.");
                    }
                    // Get a stream containing an image of a cat
                    if (content == "" || content.Length == 0)
                        content = "error :)";
                    var stream = await _module.CatPictureAsync(32, "", content);
                    await arg.Channel.SendFileAsync(stream, "cat.png");
                }
            }
        }
    }
}
