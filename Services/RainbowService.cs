using Discord;
using Discord.WebSocket;

using TastyBot.Contracts;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TastyBot.Services
{
    public class RainbowService : IRainbowService
    {
        private readonly DiscordSocketClient _discord;
        private readonly Random _random = new Random();

        public RainbowService(DiscordSocketClient discord)
        {
            _discord = discord;
            //we'll just change the role colors when we get a message on the discord, instead of doing it over time ;)
            _discord.MessageReceived += MessageReceivedAsync;

            //This is just to be fancy
            string rainbow = "started rainbow service";
            var count = Enum.GetNames(typeof(ConsoleColor)).Length;
            foreach (char c in rainbow)
            {
                Console.ForegroundColor = (ConsoleColor)typeof(ConsoleColor).GetEnumValues().GetValue(_random.Next(0, count));
                Console.Write(c);
            }
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green; //ye we'll just keep it green m'kay? Hack the planet!

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

        public IEnumerable<Color> CreateRainbowColors(string textToRainbow)
        {
            List<Color> colors = new List<Color>();

            foreach (char c in textToRainbow.ToCharArray())
            {
                colors.Add(new Color(_random.Next(0, 255), _random.Next(0, 255), _random.Next(0, 255)));
            }

            return colors;
        }
    }
}
