using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;

namespace TastyBot.Services
{
    public class CommandHandlingService
    {
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _discord;
        private readonly IServiceProvider _provider;
        private readonly IConfigurationRoot _config;


        //much neater
        public CommandHandlingService(DiscordSocketClient discord, CommandService commands, IConfigurationRoot config, IServiceProvider provider)
        {
            _discord = discord;
            _commands = commands;
            _config = config;
            _provider = provider;

            //Hook Messages so we can process them if they're commands.
            _discord.MessageReceived += OnMessageReceivedAsync;
            _commands.CommandExecuted += OnCommandExecuted;

        }

        private async Task OnCommandExecuted(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            if (!command.IsSpecified) //if the command does not exist, truncate response
                return;

            if (!result.IsSuccess) // If not successful, reply with the error.
                await context.Channel.SendMessageAsync(result.ToString());
        }

        private async Task OnMessageReceivedAsync(SocketMessage s)
        {
            var msg = s as SocketUserMessage;                           // Ensure the message is from a user/bot
            if (msg == null) return;
            if (msg.Author.Id == _discord.CurrentUser.Id) return;       // Ignore self when checking commands

            var context = new SocketCommandContext(_discord, msg);      // Create the command context

            int argPos = 0;                                             // Check if the message has a valid command prefix
            if (msg.HasStringPrefix(_config["prefix"], ref argPos) || msg.HasMentionPrefix(_discord.CurrentUser, ref argPos))
            {

                var result = await _commands.ExecuteAsync(context, argPos, _provider);      // Execute the command
            }
        }
    }
}