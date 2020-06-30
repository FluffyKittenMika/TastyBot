using Discord;
using Discord.Commands;

using TastyBot.Services;
using TastyBot.Utility;

using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System;
using TastyBot.Contracts;

namespace TastyBot.Modules
{
    // Modules must be public and inherit from an IModuleBase
    [Name("General Commands")]
    public class PublicModule : ModuleBase<SocketCommandContext>
    {
		// Dependency Injection will fill this value in for us
		public RainbowService RainbowService { get; set; }
		public CatBotService CatBotService { get; set; }


		private readonly CommandService _service;

        public PublicModule(CommandService service)
        {
            _service = service;
        }
		

		// Ban a user
		[Command("ban")]
		[RequireContext(ContextType.Guild)]
		// make sure the user invoking the command can ban
		[RequireUserPermission(GuildPermission.BanMembers)]
		// make sure the bot itself can ban
		[RequireBotPermission(GuildPermission.BanMembers)]
		public async Task BanUserAsync(IGuildUser user, [Remainder] string reason = null)
		{
			await user.Guild.AddBanAsync(user, reason: reason);
			await ReplyAsync("ok!");
		}


		// Throw Back Thursday, Tasty Specified command.
		[Command("tbt")]
		[Summary("Use this command once every thursday :D")]
		public Task ThrowBackThrsday()
			=> ReplyAsync("<:Tastyderp:669202378095984640> Throwback Thursday! Post an old picture of you and your friends in #Photos!");

		[Command("nnm")]
		[Summary("Use this command once very monday :D")]
		public Task NomNomMonday()
			=> ReplyAsync("<:Oposum:669676896270680104> Nom Nom Monday! Post a picture of your food in #Photos!");
		
		// Get info on a user, or the user who invoked the command if one is not specified
		[Command("userinfo")]
		public async Task UserInfoAsync(IUser user = null)
		{
			user ??= Context.User;
			await ReplyAsync(user.ToString());
		}

		/* Commenting out theese commands as they're really not needed, keeping them around for reference. 
		 
		
		// [Remainder] takes the rest of the command's arguments as one argument, rather than splitting every space
		[Command("echo")]
		public Task EchoAsync([Remainder] string text)
			// Insert a ZWSP before the text to prevent triggering other bots!
			=> ReplyAsync('\u200B' + text);
		

		// 'params' will parse space-separated elements into a list
		[Command("list")]
		public Task ListAsync(params string[] objects)
			=> ReplyAsync("You listed: " + string.Join("; ", objects));

		// Setting a custom ErrorMessage property will help clarify the precondition error
		[Command("guild_only")]
		[RequireContext(ContextType.Guild, ErrorMessage = "Sorry, this command must be ran from within a server, not a DM!")]
		public Task GuildOnlyCommand()
			=> ReplyAsync("Nothing to see here!");
		*/


	}
}