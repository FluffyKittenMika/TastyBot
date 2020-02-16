using Discord;
using Discord.Commands;
using Microsoft.Extensions.Configuration;
using TastyBot.Services;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System;

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
		[Summary(" sends a pic of a cat\n\nFor a cat pic write:\n!cat\n\nfor a cat gif write either:\n!cat g or !cat gif\n\nfor a cat pic with text write:\n!cat t {some text here}\n\nfor a cat pic with colored text write:\n!cat t {some text here} c {color name}\n\nfor a cat pic with size adjustments and color write:\n!cat t {some text here} c {color name} s {size number}")]

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
				if (args.ElementAt(0).ToLower() == "gif" || args.ElementAt(0).ToLower() == "g")
				{
					// Get a stream containing an image of a cat
					var stream = await PictureService.GetCatGifAsync();
					// Streams must be seeked to their beginning before being uploaded!
					stream.Seek(0, SeekOrigin.Begin);
					await Context.Channel.SendFileAsync(stream, "cat.gif");
				}
				else
				{
					if (args.ElementAt(0).ToLower() == "txt" || args.ElementAt(0).ToLower() == "t")
					{
						if (args.Length == 1)
						{
							await ReplyAsync("Splish splash, you forgot to add some text");
						}
						else
						{
							if (args.ElementAt(1).ToLower() == "c" || args.ElementAt(1).ToLower() == "color")
							{
								await ReplyAsync("Splish splash, you forgot to add some text");
							}
							else
							{
								int IndexNum = 2;
								int ColorPos = 0;
								int Lenght = args.Length - 1;
								bool DoWhile = true;
								do
								{
									if (Lenght >= IndexNum)
									{
										if (args.ElementAt(IndexNum).ToLower() == "color" || args.ElementAt(IndexNum).ToLower() == "c")
										{
											ColorPos = IndexNum + 1;

										}
										else
										{
											IndexNum = 1 + IndexNum;
											continue;
										}
									}
									if (string.IsNullOrEmpty(args.ElementAt(ColorPos)))
									{
										await ReplyAsync("Splish splash, you forgot to add a color name");
									}
									else
									{
										if (ColorPos == 0)
										{
											int Num1 = 1;
											string TextVar = args.ElementAt(Num1);
											if (args.Length == 2)
											{
												var stream = await PictureService.GetCatPictureWTxtAsync(TextVar);
												// Streams must be seeked to their beginning before being uploaded!
												stream.Seek(0, SeekOrigin.Begin);
												await Context.Channel.SendFileAsync(stream, "cat.png");
											}
											else
											{
												int len = args.Length - 1;
												do
												{

													Num1 = ++Num1;
													TextVar = TextVar + " " + args.ElementAt(Num1);

												} while (Num1 < len);
												var stream = await PictureService.GetCatPictureWTxtAsync(TextVar);
												// Streams must be seeked to their beginning before being uploaded!
												stream.Seek(0, SeekOrigin.Begin);
												await Context.Channel.SendFileAsync(stream, "cat.png");
											}
										}
										else
										{
											int Num1 = 1;
											string TextVar = args.ElementAt(Num1);
											int len = ColorPos - 2;
											do
											{

												Num1 = ++Num1;
												if (!(args.ElementAt(Num1).ToLower() == "c" || args.ElementAt(Num1).ToLower() == "color"))
												{
													TextVar = TextVar + " " + args.ElementAt(Num1);
												}
												

											} while (Num1 < len);
											int LenghtTot = args.Length - 2;
											if (args.ElementAt(LenghtTot).ToLower() == "s" || args.ElementAt(LenghtTot).ToLower() == "size")
											{
												LenghtTot = Convert.ToInt32(LenghtTot) + 1;
												var stream = await PictureService.GetCatPictureWTxtAsyncAndColor(TextVar, args.ElementAt(ColorPos), Convert.ToInt32(args.ElementAt(LenghtTot)));
												// Streams must be seeked to their beginning before being uploaded!
												stream.Seek(0, SeekOrigin.Begin);
												await Context.Channel.SendFileAsync(stream, "cat.png");
											}
											else
											{
												var stream = await PictureService.GetCatPictureWTxtAsyncAndColor(TextVar, args.ElementAt(ColorPos), 50);
												// Streams must be seeked to their beginning before being uploaded!
												stream.Seek(0, SeekOrigin.Begin);
												await Context.Channel.SendFileAsync(stream, "cat.png");
											}
											
										}
									}

									DoWhile = false;
								} while (DoWhile);
							}


							// Get a stream containing an image of a cat
						}
					}
					else
					{
						await ReplyAsync("Splish splash, you forgot to add a color name");
					}
				}
				
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