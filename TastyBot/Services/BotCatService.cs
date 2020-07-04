using Discord;
using Discord.WebSocket;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TastyBot.Contracts;

namespace TastyBot.Services
{
    public class BotCatService
    {
        private readonly DiscordSocketClient _discord;
        private readonly IPictureService _pictureServ;

        public BotCatService(IPictureService pictureServ, DiscordSocketClient discord)
        {
            _discord = discord;
            //we'll just change the role colors when we get a message on the discord, instead of doing it over time ;)
            _discord.MessageReceived += OnMessageReceivedAsync;
            _pictureServ = pictureServ;
            Console.WriteLine("Botcat service ready!");
        }

        public async Task OnMessageReceivedAsync(SocketMessage arg)
        {
            // Ignore system messages, or messages from other bots
            if (!(arg is SocketUserMessage message)) return;
            if (message.Source != MessageSource.User) return;

            //Catbot channel
            if (arg.Channel.Name.ToLower() == "botcat") //More generic
            {
                //just to keep a basic console log
                Console.WriteLine($"catbot:{message.Author} - {message.Content.ToLower()} - cattified :3");

                //remove the evicence
                await arg.DeleteAsync();

                if (arg.Content.Length > 0 && arg.Content != null)
                {
                    string content = arg.Content;
                    // Replace invalid characters with empty strings.
                    try
                    {
                        content = Regex.Replace(content, @"[^a-zA-Z0-9 ]+", "", RegexOptions.None, TimeSpan.FromSeconds(5));
                        Console.WriteLine("Regexed into: " + content);
                    }
                    catch (TimeoutException)
                    {
                        //let the masses know that home hosting is not always so great, but it beats spending money on an dedicated machine, but i'm seriously considering making a community hosted server lol.
                        await arg.Channel.SendMessageAsync("Can't get a cat atm, we're experiencing connection issues and we're timing out. Tastybot crew is sorry for this atrocity. And we will punish the ISP gods promptly. Also @Mikaelssen if nothing happens.");
                    }
                    // Get a stream containing an image of a cat
                    if (content == "" || content.Length == 0)
                        content = "error :)";
                    var stream = await _pictureServ.GetCatPictureAsync(content, "", 1);
                    // Streams must be seeked to their beginning before being uploaded!
                    stream.Seek(0, SeekOrigin.Begin);
                    await arg.Channel.SendFileAsync(stream, "cat.png");
                }
            }
        }
    }
}
