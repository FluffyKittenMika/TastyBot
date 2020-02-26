using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Reflection;
using System.Threading.Tasks;

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

            if (msg.Content.Contains("debug") || msg.Content.Contains("Debug"));
            {
                if (msg.Author.Id == 83183880869253120 || msg.Author.Id == 277038010862796801 || msg.Author.Id == 457461440429817877)
                {
                    string SMsg = Convert.ToString(msg);
                    int SMsgL = SMsg.Length;
                    string STimeS = Convert.ToString(SMsg);
                    if (SMsgL == 8)
                    {
                        
                        int STimeSI = Convert.ToInt32(STimeS[7]);
                        await Task.Delay(STimeSI * 1000);
                    }
                    else
                    {
                        if (SMsgL == 9)
                        {
                            string STimeSI = $"{STimeS[7]}{STimeS[8]}";
                            int STimeSII = Convert.ToInt32(STimeSI);
                            await Task.Delay(STimeSII * 1000);
                        }
                        else
                        {
                            if (SMsgL == 10)
                            {
                                string STimeSI = $"{STimeS[7]}{STimeS[8]}{STimeS[9]}";
                                int STimeSII = Convert.ToInt32(STimeSI);
                                if (!((STimeSII / 60) >= 2))
                                {
                                    await Task.Delay(STimeSII * 1000);
                                }
                                
                                
                            }
                        }
                    }
                }
            }
            
            int argPos = 0;                                             // Check if the message has a valid command prefix
            if (msg.HasStringPrefix(_config["prefix"], ref argPos) || msg.HasMentionPrefix(_discord.CurrentUser, ref argPos))
            {

                var result = await _commands.ExecuteAsync(context, argPos, _provider);      // Execute the command
            }
        }
    }
}