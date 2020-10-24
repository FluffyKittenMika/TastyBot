using Discord;
using Discord.Commands;
using Discord.WebSocket;
using MasterMind.Contracts;
using System;
using System.Threading.Tasks;
using System.IO;
using Discord.Rest;
using System.Collections.Generic;
using System.Linq;
using Utilities.TasksUtilities;
using System.Drawing;
using Color = System.Drawing.Color;
using System.Threading;
using BusinessLogicLayer.Services;

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

        public MasterMindModule(IMasterMindModule module, DiscordSocketClient discord) 
        {
            _module = module;
            _discord = discord;
            _discord.ReactionAdded += OnReactionAddedAsync;
        }


        private async Task OnReactionAddedAsync(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
        {
            
            if (arg3.Channel.Name.ToLower() != "master-mind") return;
            if (arg3.User.Value.IsBot) return;
            if (_module.IsSecondReactionAdded(arg2, arg3))
            {
                return;
            }
            if (!_module.IsEmojiAllowed(new Emoji(arg3.Emote.Name.ToString())))
            {
                RestUserMessage message = await arg3.Channel.SendMessageAsync($"Very much error has occurred, <@{arg3.UserId}> the emote inserted was not recognised. Please insert a valid emote");
                _module.DeleteMessage(5, message).PerformAsyncTaskWithoutAwait();
                var rMessage = await message.Channel.GetMessageAsync(arg3.MessageId);
                rMessage.RemoveReactionAsync(arg3.Emote, arg3.User.Value.Id).PerformAsyncTaskWithoutAwait();
                return;
            }
            if (!_module.ReactionOnRightMessage(arg3.MessageId, arg3.User.Value))
            {
                RestUserMessage message = await arg3.Channel.SendMessageAsync($"Very much error has occurred, <@{arg3.UserId}> please dont add an emote to someone elses game");
                _module.DeleteMessage(5, message).PerformAsyncTaskWithoutAwait();
                var rMessage = await message.Channel.GetMessageAsync(arg3.MessageId);
                rMessage.RemoveReactionAsync(arg3.Emote, arg3.User.Value.Id).PerformAsyncTaskWithoutAwait();
                return;

            }
            if (_module.IsAnArrow(new Emoji(arg3.Emote.Name.ToString())))
            {
                if (_module.CanArrowBeUsed(arg3.User.Value))
                {
                    if (_module.DidUserWin(arg3.User.Value))
                    {
                        await arg3.Channel.SendMessageAsync($"<@{arg3.UserId}> Congratulations, you just won a game of mastermind");
                    }
                    if (_module.DidUserLose(arg3.User.Value))
                    {
                        await arg3.Channel.SendMessageAsync($"<@{arg3.UserId}> You lost, better luck next time");
                    }
                    MemoryStream stream = new MemoryStream();
                    Bitmap bitmap = _module.RunGame(new Emoji(arg3.Emote.Name.ToString()), arg3.User.Value);
                    bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    stream.Seek(0, SeekOrigin.Begin);
                    RestUserMessage message = await arg3.Channel.SendFileAsync(stream, $"MasterMind.png");
                    SendEmote(message);
                    _module.SaveMessage(message, arg3.User.Value);
                    stream.Dispose();
                }
                else
                {
                    RestUserMessage message = await arg3.Channel.SendMessageAsync($"Very much error has occurred, <@{arg3.UserId}> you must have completed a full line before passing to the next one");
                    var rMessage = await message.Channel.GetMessageAsync(arg3.MessageId);
                    rMessage.RemoveReactionAsync(arg3.Emote, arg3.User.Value.Id).PerformAsyncTaskWithoutAwait();
                    _module.DeleteMessage(5, message).PerformAsyncTaskWithoutAwait();
                    return;
                }
            }
            else
            {
                MemoryStream stream = new MemoryStream();
                Bitmap bitmap = _module.RunGame(new Emoji(arg3.Emote.Name.ToString()), arg3.User.Value);
                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                stream.Seek(0, SeekOrigin.Begin);
                RestUserMessage message = await arg3.Channel.SendFileAsync(stream, $"MasterMind.png");
                SendEmote(message);
                _module.SaveMessage(message, arg3.User.Value);
                stream.Dispose();
            }

        }
        
        [Command("enablemm")]
        [Alias("emm")]
        [Summary("Creates a channel named master-mind, to use mastermind on")]
        public async Task EnableMm()
        {

            foreach (var channel in Context.Guild.Channels)
            {
                if (channel.Name.ToLower() == "master-mind")
                {
                    await Context.Channel.SendMessageAsync($"<@{Context.User.Id}> channel already exists");
                    return;
                }
            }
            await Context.Guild.CreateTextChannelAsync("master-mind");
        }
        [Command("wins")]
        [Alias("w")]
        [Summary("Shows the amount of wins a user has achieved on mastermind")]
        public async Task GetUserWins(IUser user = null)
        {
            long win;
            if (user == null)
            {
                win = _module.GetUserWins(Context.User);
                    
            } 
            else
            {
                win = _module.GetUserWins(user);
            }
            
            await Context.Channel.SendMessageAsync($"The user has {win} win[s]");
            
        }

        [Command("start")]
        [Summary("starts game of mastermind\nto start a game do !start\nfor additional customization do !start {height} {width} {color}")]
        
        public async Task StartMasterMind(int height = 12, int width = 4, string color = "e")
        {
            if (Context.Channel.Name.ToLower() != "master-mind") return;
            if (_module.UserHasRunningGame(Context.User))
            {
                var message = await Context.Channel.SendMessageAsync($"Very much error has occured, <@{Context.User.Id}> Don't start a game while a game is in process");
                _module.DeleteMessage(5, message).PerformAsyncTaskWithoutAwait();
                return;
            }
            if (height > 30)
            {
                var message = await Context.Channel.SendMessageAsync($"<@{Context.User.Id}> The height has been clamped to 30 dots, because why the hell would you need more?");
                _module.DeleteMessage(5, message).PerformAsyncTaskWithoutAwait();
            }
            if (width > 10)
            {
                var message = await Context.Channel.SendMessageAsync($"<@{Context.User.Id}> The width has been clamped to 10 dots, because why the hell would you need more?");
                _module.DeleteMessage(5, message).PerformAsyncTaskWithoutAwait();
            }
            if (height < 1)
            {
                var message = await Context.Channel.SendMessageAsync($"<@{Context.User.Id}> The height has been clamped to 1 dot, how would you play without any dots?");
                _module.DeleteMessage(5, message).PerformAsyncTaskWithoutAwait();
            }
            if (width < 1)
            {
                var message = await Context.Channel.SendMessageAsync($"<@{Context.User.Id}> The width has been clamped to 1 dot, how would you play without any dots?");
                _module.DeleteMessage(5, message).PerformAsyncTaskWithoutAwait();
            }
            height = Math.Clamp(height, 1, 30);
            width = Math.Clamp(width, 1, 10);
            Color ColorIndicator = Color.FromName(color);
            if (ColorIndicator.A == 0 && ColorIndicator.R == 0 && ColorIndicator.G == 0 && ColorIndicator.B == 0 && color != "e")
            {
                await Context.Channel.SendMessageAsync($"<@{Context.User.Id}> Error in searching the color sent, color will be defualted to red");

            }
            Bitmap bitPicture = _module.StartGame(width, height, Context.User, ColorIndicator);
            MemoryStream streamPicture = new MemoryStream();
            bitPicture.Save(streamPicture, System.Drawing.Imaging.ImageFormat.Png);
            streamPicture.Seek(0, SeekOrigin.Begin);
            RestUserMessage imageMessage = await Context.Channel.SendFileAsync(streamPicture, "MasterMind.png");

            _module.SaveMessage(imageMessage, Context.User);
            /*  
            _module.StartGame();
            stream = _module.StartBoardMaker(height, width);
            

            imageMessage = await Context.Channel.SendFileAsync(stream, "MasterMind.png");
            string imageUrl = imageMessage.Attachments.FirstOrDefault().Url;
            await Context.Channel.SendMessageAsync(imageUrl);
            */
            SendEmote(imageMessage);
            streamPicture.Dispose();
            /* Task.Delay(6000);
            await imageMessage.ModifyAsync(msg => msg.Content = "test [edited]"); */

        }
        
        public void SendEmote(RestUserMessage imageMessage)
        {
            
            Emoji YourEmoji = new Emoji("⚫");
            Task Message1 = imageMessage.AddReactionAsync(YourEmoji);
            Emoji YourEmoji1 = new Emoji("🟡");
            Task Message2 = imageMessage.AddReactionAsync(YourEmoji1);
            Emoji YourEmoji2 = new Emoji("🟠");
            Task Message3 = imageMessage.AddReactionAsync(YourEmoji2);
            Emoji YourEmoji3 = new Emoji("🟣");
            Task Message4 = imageMessage.AddReactionAsync(YourEmoji3);
            Emoji YourEmoji4 = new Emoji("🟢");
            Task Message5 = imageMessage.AddReactionAsync(YourEmoji4); 
            Emoji YourEmoji5 = new Emoji("🔵");
            Task Message6 = imageMessage.AddReactionAsync(YourEmoji5);
            Emoji YourEmoji6 = new Emoji("🔴");
            Task Message7 = imageMessage.AddReactionAsync(YourEmoji6);
            Task[] tasks = new Task[7] { Message1, Message2, Message3, Message4, Message5, Message6, Message7 };
            Task.WaitAll(tasks);
            Emoji YourEmoji7 = new Emoji("➡️");
            imageMessage.AddReactionAsync(YourEmoji7);
        }
        
    }
}
