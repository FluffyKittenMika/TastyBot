using Discord;
using Discord.Commands;
using Discord.WebSocket;
using MasterMind.Contracts;
using System;
using System.Threading.Tasks;
using System.IO;
using Discord.Rest;
using Utilities.TasksManager;
using System.Collections.Generic;
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
        private readonly IMasterMindService _service;
        RestUserMessage imageMessage;
        RestUserMessage imageMessageUrl;
        List<int> colorGuessemote = new List<int>();
        public MasterMindModule(IMasterMindModule module, DiscordSocketClient discord, IMasterMindService service) 
        {
            _module = module;
            _discord = discord;
            _service = service;
            _discord.ReactionAdded += OnReactionAddedAsync;
        }

        private async Task OnReactionAddedAsync(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
        {
            if (arg3.Channel.Name.ToLower() != "master mind") return;
            if (arg3.User.Value.IsBot) return;
            //need to add an if to see if the reaction was added by whoever was playing
            
            
            
        }

        [Command("start")]
        [Summary("starts game of mastermind\nto start a game do !start\nfor additional customization do !start {height} {width}")]
        
        public async Task StartMasterMind(int height = 12, int width = 4)
        {
            
            if (_module.GameIsRunningM(Context.User))
            {
                await Context.Channel.SendMessageAsync("Very much error has occured, Don't start a game while a game is in process");
                return;
            }
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
        
            //Is useless
            height = Math.Clamp(height, 0, 30);
            width = Math.Clamp(width, 0, 10);


            /*
            _module.StartGame();
            stream = _module.StartBoardMaker(height, width);
            

            imageMessage = await Context.Channel.SendFileAsync(stream, "MasterMind.png");
            string imageUrl = imageMessage.Attachments.FirstOrDefault().Url;
            await Context.Channel.SendMessageAsync(imageUrl);
            */
            SendEmote();
            /* Task.Delay(6000);
            await imageMessage.ModifyAsync(msg => msg.Content = "test [edited]"); */

        }
        public void SendEmote()
        {
            Emoji YourEmoji = new Emoji("⚫");
            imageMessage.AddReactionAsync(YourEmoji).PerformAsyncTaskWithoutAwait();
            Emoji YourEmoji1 = new Emoji("🟡");
            imageMessage.AddReactionAsync(YourEmoji1).PerformAsyncTaskWithoutAwait();
            Emoji YourEmoji2 = new Emoji("🟠");
            imageMessage.AddReactionAsync(YourEmoji2).PerformAsyncTaskWithoutAwait();
            Emoji YourEmoji3 = new Emoji("🟣");
            imageMessage.AddReactionAsync(YourEmoji3).PerformAsyncTaskWithoutAwait();
            Emoji YourEmoji4 = new Emoji("🟢");
            imageMessage.AddReactionAsync(YourEmoji4).PerformAsyncTaskWithoutAwait();
            Emoji YourEmoji5 = new Emoji("🔵");
            imageMessage.AddReactionAsync(YourEmoji5).PerformAsyncTaskWithoutAwait();
            Emoji YourEmoji6 = new Emoji("🔴");
            imageMessage.AddReactionAsync(YourEmoji6).PerformAsyncTaskWithoutAwait();
        }
        
    }
}
