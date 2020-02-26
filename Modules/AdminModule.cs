using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;
using TastyBot.Services;
using System.Linq;
using System.IO;
using TastyBot.Utility;

namespace TastyBot.Modules
{
    [Name("Admin")]
    public class AdminModule : ModuleBase<SocketCommandContext>
    {


        [Command("debug")]
        [Summary("stops the bot for an amount of time")]
        public async Task Debug(params string[] Txt)
        {
            
            if (Context.User.Id != 83183880869253120 && Context.User.Id != 277038010862796801 && Context.User.Id != 457461440429817877)
            {
                await ReplyAsync("bippity boppity, you dont have the permission for this.");
                return;

            }
            

            

        }
        
        
    }
}
