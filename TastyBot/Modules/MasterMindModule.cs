using Discord;
using Discord.Commands;
using Discord.WebSocket;
using MasterMind.Contracts;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Net.Http;
using TastyBot.Contracts;
using MasterMind.Modules;
using Discord.Rest;
using System.Linq;

namespace TastyBot.Modules
{
    /// <summary>
    /// The Module that handles all MasterMind Things
    /// </summary>
    [Name("MasterMind")]
    public class MasterMindModule : ModuleBase<SocketCommandContext>
    {
        private readonly IMasterMindModule _module;
        private readonly DiscordSocketClient _discord;
        RestUserMessage imageMessage;
        public MasterMindModule(IMasterMindModule module, DiscordSocketClient discord) 
        {
            _module = module;
            _discord = discord;
            _discord.ReactionAdded += OnReactionAddedAsync;
        }
        
        [Command("start")]
        [Summary("starts game of mastermind\nto start a game do !start\nfor additional customization do !start {height} {width}")]
        
        public async Task StartMasterMind(int height = 12, int width = 4)
        {
            MemoryStream stream;
            if (Context.Channel.Name.ToLower() != "master-mind") return;
            if (height > 30)
            {
                await Context.Channel.SendMessageAsync("The height has been clamped to 30 dots, because why the hell would you need more?");
            }
            if (width > 10)
            {
                await Context.Channel.SendMessageAsync("The width has been clamped to 10 dots, because why the hell would you need more?");
            }
        
            height = Math.Clamp(height, 0, 30);
            width = Math.Clamp(width, 0, 10);

            if (_module.IsGameRunning())
            {
                await Context.Channel.SendMessageAsync("Very much error has occured, Don't start a game while a game is in process");
                return;
            }

            await _module.StartGame();
            stream = await _module.StartBoardMaker(height, width);
            imageMessage = await Context.Channel.SendFileAsync(stream, "MasterMind.png");
            await SendEmote();
            /* Task.Delay(6000);
            await imageMessage.ModifyAsync(msg => msg.Content = "test [edited]"); */

        }
        public async Task SendEmote()
        {
            Emoji YourEmoji = new Emoji("⚫");
            imageMessage.AddReactionAsync(YourEmoji);
            Emoji YourEmoji1 = new Emoji("🟡");
            imageMessage.AddReactionAsync(YourEmoji1);
            Emoji YourEmoji2 = new Emoji("🟠");
            imageMessage.AddReactionAsync(YourEmoji2);
            Emoji YourEmoji3 = new Emoji("🟣");
            imageMessage.AddReactionAsync(YourEmoji3);
            Emoji YourEmoji4 = new Emoji("🟢");
            imageMessage.AddReactionAsync(YourEmoji4);
            Emoji YourEmoji5 = new Emoji("🔵");
            imageMessage.AddReactionAsync(YourEmoji5);
            Emoji YourEmoji6 = new Emoji("🔴");
            imageMessage.AddReactionAsync(YourEmoji6);
        }
        
    }
}
