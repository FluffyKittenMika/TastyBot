using Discord;
using Discord.Commands;
using Microsoft.Extensions.Configuration;
using TastyBot.Services;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace TastyBot.Modules
{
    // Modules must be public and inherit from an IModuleBase
    [Name("General Commands")]
    public class PublicModule : ModuleBase<SocketCommandContext>
    {
        // Dependency Injection will fill this value in for us
        public PictureService PictureService { get; set; }
        public RainbowService RainbowService { get; set; }

        
        private readonly CommandService _service;
        private readonly IConfigurationRoot _config;

        public PublicModule(CommandService service, IConfigurationRoot config)
        {
            _service = service;
            _config = config;
        }

		[Command("cat", true)]
		public async Task CatAsync(params string[] args)
		{
			if (args.Length == 0 || args == null)
			{
				// Get a stream containing an image of a cat
				var stream = await PictureService.GetCatPictureAsync();
				// Streams must be seeked to their beginning before being uploaded!
				stream.Seek(0, SeekOrigin.Begin);
				await Context.Channel.SendFileAsync(stream, "cat.png");
			}
			else
			{
				// Get a stream containing an image of a cat
				var stream = await PictureService.GetCatGifAsync();
				// Streams must be seeked to their beginning before being uploaded!
				stream.Seek(0, SeekOrigin.Begin);
				await Context.Channel.SendFileAsync(stream, "cat.gif");
			}
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
		public Task ThrowBackThrsday()
			=> ReplyAsync("<:Tastyderp:669202378095984640> Throwback Thursday! Post an old picture of you and your friends in #Photos!");


		
		// Get info on a user, or the user who invoked the command if one is not specified
		[Command("userinfo")]
		public async Task UserInfoAsync(IUser user = null)
		{
			user = user ?? Context.User;
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