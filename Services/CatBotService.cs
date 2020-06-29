using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;

namespace TastyBot.Services
{
    public class CatBotService
    {
        private readonly DiscordSocketClient _discord;
        private readonly PictureService _p;

        public CatBotService(DiscordSocketClient discord, PictureService P)
        {
            _discord = discord;
            _p = P;

            //we'll just change the role colors when we get a message on the discord, instead of doing it over time ;)
            _discord.MessageReceived += MessageReceivedAsync;

        }

        private async Task MessageReceivedAsync(SocketMessage arg)
        {
            // Ignore system messages, or messages from other bots
            if (!(arg is SocketUserMessage message)) return;
            if (message.Source != MessageSource.User) return;

            //Catbot channel
            if (arg.Channel.Name.ToLower() == "catbot") //More generic
            {
                PictureService p;
                await arg.DeleteAsync();
                await _p.GetCatPictureWTxtAsync(arg.Content);


                if (arg.Content.Length > 0 && arg.Content != null)
                {
                    // Get a stream containing an image of a cat
                    var stream = await _p.GetCatPictureWTxtAsync(arg.Content);
                    // Streams must be seeked to their beginning before being uploaded!
                    stream.Seek(0, SeekOrigin.Begin);
                    await arg.Channel.SendFileAsync(stream, "cat.png");
                }
            }
            return;
        }
    }
}
