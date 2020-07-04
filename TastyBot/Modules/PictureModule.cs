using Discord.Commands;
using System.IO;
using System.Threading.Tasks;
using Enums.PictureServices;
using TastyBot.Contracts;
using System;
using System.Linq;
using System.Globalization;

namespace TastyBot.Modules
{
    [Name("Picture Commands")]
    public class PictureModule : ModuleBase<SocketCommandContext>
    {
        private readonly IPictureService _serv;
        static Random _R = new Random();
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
			await Context.Channel.SendFileAsync(s, "Neko.png");
		}

		[Command("nekoavatar")]
		public async Task NekoAvatarAsync([Remainder]string text = "")
		{
			var s = await _serv.GetRegularNekoClientPictureAsync(RegularNekos.Avatar, text);
			await Context.Channel.SendFileAsync(s, "Avatar.png");
		}

		[Command("nekowallpaper")]
		public async Task NekoWallpaperAsync([Remainder]string text = "")
		{
			var s = await _serv.GetRegularNekoClientPictureAsync(RegularNekos.Wallpaper, text);
			await Context.Channel.SendFileAsync(s, "Wallpaper.png");
		}

		[Command("fox")]
		public async Task FoxAsync([Remainder]string text = "")
		{
			var s = await _serv.GetRegularNekoClientPictureAsync(RegularNekos.Fox, text);
			await Context.Channel.SendFileAsync(s, "Fox.png");
		}

        #endregion

        #region NSFW Nekos owo

        //TODO: ADD command descriptors to all commands in this module.
        [Command("NSFW")]
        [RequireNsfw]
        public async Task NSFWAhegaoAsync([Remainder]string text = "")
        {

            string E = text;
            NSFWNekos res;

            if (E == "")
                res = RandomEnumValue<NSFWNekos>();
            else
            {
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                E = textInfo.ToTitleCase(text.Split(' ').FirstOrDefault());
                Console.WriteLine(E);
                Enum.TryParse(E, out res);
            }
            text = string.Join(' ', text.Split(' ').Skip(1));

            var s = await _serv.GetNSFWNekoClientPictureAsync(res, text);
            await Context.Channel.SendFileAsync(s, "OwO.png");
        }
        #endregion
  
        static T RandomEnumValue<T>()
        {
            var v = Enum.GetValues(typeof(T));
            return (T)v.GetValue(_R.Next(v.Length));
        }

    }

}