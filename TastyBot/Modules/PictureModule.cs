using Discord.Commands;
using System.IO;
using System.Threading.Tasks;
using System.Drawing;
using Enums.PictureServices;
using TastyBot.Contracts;
using TastyBot.Services;

namespace TastyBot.Modules
{
    [Name("Picture Commands")]
    public class PictureModule : ModuleBase<SocketCommandContext>
    {
        private readonly IPictureService _serv;

        public PictureModule(IPictureService serv)
        {
            _serv = serv;
        }

        #region Cat meow meow region
        [Command("cat")]
        public async Task CatAsync(int textsize = 32, string Colour = "white", [Remainder] string text = " ")
        {
            var s = await _serv.GetCatPictureAsync(text, Colour, textsize);
            s.Seek(0, SeekOrigin.Begin);
            await Context.Channel.SendFileAsync(s, "cat.png");
        }


        [Command("cat")]
        public async Task CatAsync(string Colour = "white", [Remainder] string text = " ")
        {
           

            if (Colour.ToLower() != "gif")
            {
                var s = await _serv.GetCatPictureAsync(text, Colour, 32);
                s.Seek(0, SeekOrigin.Begin);
                await Context.Channel.SendFileAsync(s, "cat.png");
            }
            else
            {
                var s = await _serv.GetCatGifAsync();
                s.Seek(0, SeekOrigin.Begin);
                await Context.Channel.SendFileAsync(s, "cat.gif");
            }
        }

        [Command("cat")]
        public async Task CatAsync([Remainder] string text = " ")
        {
            var s = await _serv.GetCatPictureAsync(text, "white", 32);
            s.Seek(0, SeekOrigin.Begin);
            await Context.Channel.SendFileAsync(s, "cat.png");
        }
        #endregion 

        #region Nekos

        [Command("neko")]
		public async Task NekoAsync([Remainder]string text = "")
		{
			var s = await _serv.GetRegularNekoClientPictureAsync(RegularNekos.Neko, text);
			s.Seek(0, SeekOrigin.Begin);
			await Context.Channel.SendFileAsync(s, "Neko.png");
		}

		[Command("nekoavatar")]
		public async Task NekoAvatarAsync([Remainder]string text = "")
		{
			var s = await _serv.GetRegularNekoClientPictureAsync(RegularNekos.Avatar, text);
			s.Seek(0, SeekOrigin.Begin);
			await Context.Channel.SendFileAsync(s, "Avatar.png");
		}

		[Command("nekowallpaper")]
		public async Task NekoWallpaperAsync([Remainder]string text = "")
		{
			var s = await _serv.GetRegularNekoClientPictureAsync(RegularNekos.Wallpaper, text);
			s.Seek(0, SeekOrigin.Begin);
			await Context.Channel.SendFileAsync(s, "Wallpaper.png");
		}

		[Command("fox")]
		public async Task FoxAsync([Remainder]string text = "")
		{
			var s = await _serv.GetRegularNekoClientPictureAsync(RegularNekos.Fox, text);
			s.Seek(0, SeekOrigin.Begin);
			await Context.Channel.SendFileAsync(s, "Fox.png");
		}

        #endregion
    }
}