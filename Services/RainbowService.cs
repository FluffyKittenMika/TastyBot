using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace TastyBot.Services
{
    public class RainbowService
    {
        private readonly DiscordSocketClient _discord;
        private readonly IServiceProvider _services;

        public RainbowService(IServiceProvider services)
        {

            Console.WriteLine("started rainbow service");
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _services = services;

            //we'll just change the role colors when we get a message on the discord, instead of doing it over time ;)
            _discord.MessageReceived += MessageReceivedAsync; 
        }

      

        private async Task MessageReceivedAsync(SocketMessage arg)
        {
            // Ignore system messages, or messages from other bots
            if (!(arg is SocketUserMessage message)) return;
            if (message.Source != MessageSource.User) return;

            Console.WriteLine($"{message.Author} : {message.Content}");
            foreach (SocketRole role in ((SocketGuildUser)message.Author).Roles)
            {
                Console.WriteLine(role.Name);
                Console.WriteLine(role.Color.RawValue);

                if (role.Name.ToLower() == "team rainbow")
                {
                    await role.ModifyAsync(x => {
                        x.Color = Rainbow();
                    });
                }
            }

            return;
        }


        public static Color Rainbow()
        {
            Random rng = new Random();
            int r = rng.Next(0, 255);
            int g = rng.Next(0, 255);
            int b = rng.Next(0, 255);
            return new Color(r, g, b);
        }

        /*
         * OK SO THIS WILL WORK, JUST SLOW AS HECK
        private static float pp = 0.1f;
        public static Color Rainbow()
        {
            float div = Math.Abs(pp % 1) * 6;
            int ascending = (int)(div % 1 * 255);
            int descending = 255 - ascending;
            pp += 125f;

            return ((int)div) switch
            {
                0 => new Color(255, 255, ascending),
                1 => new Color(descending, 255, 0),
                2 => new Color(0, 255, ascending),
                3 => new Color(0, descending, 255),
                4 => new Color(ascending, 0, 255),
                _ => new Color(255, 0, descending),
            };
        }
        */
    }
}
