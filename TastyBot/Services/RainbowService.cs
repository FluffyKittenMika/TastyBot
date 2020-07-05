using Discord;
using Discord.WebSocket;

using System;
using System.Threading.Tasks;
using TastyBot.Contracts;

namespace TastyBot.Services
{
    public class RainbowService : IRainbowService
    {
        private readonly Random _random = new Random();

        private readonly ILoggingService _log;
        private readonly string _logSource;

        public RainbowService(ILoggingService log, DiscordSocketClient discord)
        {
            _log = log;
            _logSource = typeof(RainbowService).Name;

            discord.MessageReceived += MessageReceivedAsync;

            _log.LogRainbowMessage(_logSource, "Ready");
        }

        private async Task MessageReceivedAsync(SocketMessage arg)
        {
            // Ignore system messages, or messages from other bots
            if (!(arg is SocketUserMessage message)) return;
            if (message.Source != MessageSource.User) return;

            //The try is here to ignore anything that we don't give a shit about :)
            try
            {
                foreach (IRole role in ((IGuildChannel)arg.Channel).Guild.Roles)
                {
                    if (role.Name.ToLower() == "team rainbow")
                    {
                        await role.ModifyAsync(x =>
                        {
                            x.Color = CreateRainbowColor();
                        });
                    }
                }
            }
            catch (Exception)
            { }

            return;
        }

        public Color CreateRainbowColor()
        {
            return new Color(_random.Next(0, 255), _random.Next(0, 255), _random.Next(0, 255));
        }

        public ConsoleColor CreateConsoleRainbowColor()
        {
            int count = Enum.GetNames(typeof(ConsoleColor)).Length;
            return (ConsoleColor)typeof(ConsoleColor).GetEnumValues().GetValue(_random.Next(0, count));
        }
    }
}
