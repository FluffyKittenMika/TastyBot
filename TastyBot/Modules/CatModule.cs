using Discord.Commands;
using System.IO;
using System.Threading.Tasks;
using System.Drawing;
using Enums.PictureServices;
using TastyBot.Contracts;

namespace TastyBot.Modules
{
    [Name("Cat Commands")]
    public class CatModule : ModuleBase<SocketCommandContext>
    {
        private readonly IPictureService _serv;

        public CatModule(IPictureService serv)
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


        //Mwahahaha tenary hell
        [Command("cat")]
        public async Task CatAsync(string Colour = "white", [Remainder] string text = " ")
        {
            /* earan hated it :c
				var s	= (Colour.ToLower() != "gif")	? await PictureService.GetCatPictureAsync(text, Colour, 32)		: await PictureService.GetCatGifAsync();
				s.Seek(0, SeekOrigin.Begin);
				_	= (Colour.ToLower() != "gif")		? await Context.Channel.SendFileAsync(s, "cat.png")				: await Context.Channel.SendFileAsync(s, "cat.gif");
			*/

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
        public async Task NekoAsync(string col = "black", [Remainder] string text = "")
        {
            Color color = Color.FromName(col);
            if (!color.IsKnownColor) //prevents it from going transparent if there's a bullshit color given, looking at you realitycat
            {
                text = col + text;
                col = "black";
            }
            var s = await _serv.GetNekoPictureAsync(RegularNekos.Neko, text, col);
            s.Seek(0, SeekOrigin.Begin);
            await Context.Channel.SendFileAsync(s, "Neko.png");
        }

        [Command("nekoavatar")]
        public async Task NekoAvatarAsync(string col = "black", [Remainder] string text = "")
        {
            Color color = Color.FromName(col);
            if (!color.IsKnownColor) //prevents it from going transparent if there's a bullshit color given, looking at you realitycat
            {
                text = col + text;
                col = "black";
            }
            var s = await _serv.GetNekoPictureAsync(RegularNekos.Avatar, text, col);
            s.Seek(0, SeekOrigin.Begin);
            await Context.Channel.SendFileAsync(s, "Avatar.png");
        }

        [Command("nekowallpaper")]
        public async Task NekoWallpaperAsync(string col = "black", [Remainder] string text = " ")
        {
            Color color = Color.FromName(col);
            if (!color.IsKnownColor) //prevents it from going transparent if there's a bullshit color given, looking at you realitycat
            {
                text = col + text;
                col = "black";
            }
            var s = await _serv.GetNekoPictureAsync(RegularNekos.Wallpaper, text, col);
            s.Seek(0, SeekOrigin.Begin);
            await Context.Channel.SendFileAsync(s, "Wallpaper.png");
        }

        [Command("fox")]
        public async Task FoxAsync(string col = "black", [Remainder] string text = " ")
        {
            Color color = Color.FromName(col);
            if (!color.IsKnownColor) //prevents it from going transparent if there's a bullshit color given, looking at you realitycat
            {
                text = col + text;
                col = "black";
            }
            var s = await _serv.GetNekoPictureAsync(RegularNekos.Fox, text, col);
            s.Seek(0, SeekOrigin.Begin);
            await Context.Channel.SendFileAsync(s, "Fox.png");
        }

        #endregion
    }
}