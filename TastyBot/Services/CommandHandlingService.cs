using TastyBot.Contracts;
using TastyBot.Utility;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.IO;

namespace TastyBot.Services
{
    public class CommandHandlingService : ICommandHandlingService
    {
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _discord;
        private readonly Config _config;
        private readonly IServiceProvider _services;


        //much neater
        public CommandHandlingService(DiscordSocketClient discord, CommandService commands, Config config, IServiceProvider services)
        {
            _discord = discord;
            _commands = commands;
            _config = config;
            _services = services;

            //Hook Messages so we can process them if they're commands.
            _discord.MessageReceived += OnMessageReceivedAsync;
            _commands.CommandExecuted += OnCommandExecuted;
        }

        public async Task OnCommandExecuted(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            if (!command.IsSpecified) //if the command does not exist, truncate response
                return;

            if (!result.IsSuccess) // If not successful, reply with the error.
                await context.Channel.SendMessageAsync(result.ToString());
        }

        public async Task OnMessageReceivedAsync(SocketMessage socketMessage)
        {
            if (!(socketMessage is SocketUserMessage msg)) return;      // Ensure the message is from a user/bot
            if (msg.Author.Id == _discord.CurrentUser.Id) return;       // Ignore self when checking commands

            var context = new SocketCommandContext(_discord, msg);      // Create the command context

            if (msg.Attachments.Any())                                  // Save all images for neural network usage if there's any attatched
            {
                Console.WriteLine("Msg with attatchments found, processing");
                foreach (var att in msg.Attachments)
                {
                    var ex = Path.GetExtension(att.Url);                                                     //Belive this works even with fancy args behind image
                    Console.WriteLine("Checking " + att.Url + "  Extension: " + ex);

                    if (ex == ".png" || ex == ".jpg")                                                        //Checks it
                    {
                        using (WebClient client = new WebClient())
                        {
                            if (!Directory.Exists("./NeuralStorage/"))
                                Directory.CreateDirectory("./NeuralStorage/");
                                
                            client.DownloadFileAsync(new Uri(att.Url), @"./NeuralStorage/" + DateTime.Now + att.Filename);  //Downloads it
                            Console.WriteLine(att.Filename + " Downloaded");
                        }
                    }
                }
            }

            int argPos = 0;                                             // Check if the message has a valid command prefix
            if (msg.HasStringPrefix(_config.Prefix, ref argPos) || msg.HasMentionPrefix(_discord.CurrentUser, ref argPos))
            {
                var result = await _commands.ExecuteAsync(context, argPos, _services);      // Execute the command
                if (!result.IsSuccess) Console.WriteLine(result.ErrorReason);
            }

        }
    }
}