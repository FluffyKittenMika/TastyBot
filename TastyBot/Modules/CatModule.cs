using Discord.Commands;
using TastyBot.Services;
using System.IO;
using System.Threading.Tasks;
using System.Drawing;
using System.Net.Http;
using System.Drawing.Imaging;

namespace TastyBot.Modules
{
    [Name("Cat Commands")]
    public class CatModule : ModuleBase<SocketCommandContext>
    {
		// Dependency Injection will fill this value in for us
		private PictureService PictureService { get; set; }

        
        private readonly CommandService _service;

        public CatModule(CommandService service)
        {
            _service = service;
			PictureService = new PictureService();
		}

		[Command("cat")]
		public async Task CatAsync(int textsize = 32, string Colour = "white", [Remainder]string text = " ")
		{
			var s	= await PictureService.GetCatPictureAsync(text, Colour, textsize);
			s.Seek(0, SeekOrigin.Begin);
			await Context.Channel.SendFileAsync( s, "cat.png");
		}


		//Mwahahaha tenary hell
		[Command("cat")]
		public async Task CatAsync(string Colour = "white", [Remainder]string text = " ")
		{
			/* earan hated it :c
				var s	= (Colour.ToLower() != "gif")	? await PictureService.GetCatPictureAsync(text, Colour, 32)		: await PictureService.GetCatGifAsync();
				s.Seek(0, SeekOrigin.Begin);
				_	= (Colour.ToLower() != "gif")		? await Context.Channel.SendFileAsync(s, "cat.png")				: await Context.Channel.SendFileAsync(s, "cat.gif");
			*/

			if (Colour.ToLower() != "gif")
			{
				var s = await PictureService.GetCatPictureAsync(text, Colour, 32);
				s.Seek(0, SeekOrigin.Begin);
				await Context.Channel.SendFileAsync(s, "cat.png");
			}
			else
			{
				var s = await PictureService.GetCatGifAsync();
				s.Seek(0, SeekOrigin.Begin);
				await Context.Channel.SendFileAsync(s, "cat.gif");
			}
		}

		[Command("cat")]
		public async Task CatAsync([Remainder]string text = " ")
		{
			var s	= await PictureService.GetCatPictureAsync(text, "white", 32);
				s.Seek(0, SeekOrigin.Begin);
				await Context.Channel.SendFileAsync(s, "cat.png");
		}

		[Command("neko")]
		public async Task NekoAsync([Remainder]string text = " ")
		{
			var s = await PictureService.GetNekoPictureAsync(text);
			s.Seek(0, SeekOrigin.Begin);
			await Context.Channel.SendFileAsync(s, "neko.png");
		}

	}
}