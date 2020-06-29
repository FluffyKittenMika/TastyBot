using Discord;
using Discord.Commands;

using TastyBot.Services;
using TastyBot.Utility;

using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System;

namespace TastyBot.Modules
{
    // TODO: HOLY FUCK REFACTOR THIS SHIT
    [Name("Cat Commands")]
    public class CatModule : ModuleBase<SocketCommandContext>
    {
        // Dependency Injection will fill this value in for us
        public PictureService PictureService { get; set; }

        
        private readonly CommandService _service;

        public CatModule(CommandService service)
        {
            _service = service;
        }

		[Command("cat", true)]
		[Summary(" sends a pic of a cat\n\nFor a cat pic write:\n!cat\n\nfor a cat gif write either:\n!cat g or !cat gif\n\nfor a cat pic with text write:\n!cat t {some text here}\n\nfor a cat pic with colored text write:\n!cat t {some text here} c {color name}\n\nfor a cat pic with size adjustments and color write:\n!cat t {some text here} c {color name} s {size number}")]
		public async Task CatAsync(params string[] args)
		{
			await Context.Message.DeleteAsync();
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
						await ReplyAsync("Splish splash, you forgot to add a color name");
				}
				
			}
		}
	}
}