using Discord.Commands;
using System.Threading.Tasks;
using Enums.PictureServices;
using System;
using System.Linq;
using System.Globalization;
using Utilities.LoggingService;
using Interfaces.Contracts.HeadpatPictures;

namespace TastyBot.Modules
{
    [Name("Picture Commands")]
    public class PictureModule : ModuleBase<SocketCommandContext>
    {
        private readonly IPictureModule _module;

        public PictureModule(IPictureModule module)
        {
            _module = module;

            Logging.LogReadyMessage(this);
        }

        #region Cat meow meow region

        [Command("cat")]
        public async Task CatAsync([Remainder] string text = "")
        {
            var stream = await _module.GetStreamFromEnumAsync(RegularCats.Cat, text);
            await Context.Channel.SendFileAsync(stream, "cat.png");
        }

        #endregion 

        #region Nekos

        [Command("neko")]
		public async Task NekoAsync([Remainder]string text = "")
		{
			var s = await _module.GetStreamFromEnumAsync(RegularNekos.Neko, text);
			await Context.Channel.SendFileAsync(s, "Neko.png");
        }

		[Command("nekoavatar")]
		public async Task NekoAvatarAsync([Remainder]string text = "")
		{
			var s = await _module.GetStreamFromEnumAsync(RegularNekos.Avatar, text);
			await Context.Channel.SendFileAsync(s, "Avatar.png");
		}

		[Command("nekowallpaper")]
		public async Task NekoWallpaperAsync([Remainder]string text = "")
		{
			var s = await _module.GetStreamFromEnumAsync(RegularNekos.Wallpaper, text);
			await Context.Channel.SendFileAsync(s, "Wallpaper.png");
		}

		[Command("fox")]
		public async Task FoxAsync([Remainder]string text = "")
		{
			var s = await _module.GetStreamFromEnumAsync(RegularNekos.Fox, text);
			await Context.Channel.SendFileAsync(s, "Fox.png");
		}

        [Command("waifu")]
        public async Task WaifuAsync([Remainder] string text = "")
        {
            var s = await _module.GetStreamFromEnumAsync(RegularNekos.Waifu, text);
            await Context.Channel.SendFileAsync(s, "Waifu.png");
        }

        #endregion

        #region NSFW Nekos owo

        #region Images
        /// <summary>
        /// Gets a random NSFW image, if not otherwise specified in input
        /// </summary>
        /// <param name="text">enum name</param>
        /// <returns>Lewd imagery directly to the discord</returns>
        [Command("NSFW")]
        [RequireNsfw]
        public async Task NSFWAsync([Remainder]string text = "")
        {
            NSFWNekos res;

            if (text == "")
                res = Utilities.Enum.Enum.RandomEnumValue<NSFWNekos>();
            else
                res = GetNSFWNekoFromString(text);

            text = string.Join(' ', text.Split(' ').Skip(1));
            var s = await _module.GetStreamFromEnumAsync(res, text);
            await Context.Channel.SendFileAsync(s, "OwO.png");
        }

        /// <summary>
        /// Attempts to get the relevant NSFW enum from a string
        /// </summary>
        /// <param name="nekoString">Input name</param>
        /// <returns>NSFWNekos</returns>
        private NSFWNekos GetNSFWNekoFromString(string nekoString)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            string NekoStringToParse = textInfo.ToTitleCase(nekoString.Split(' ').FirstOrDefault());
            Console.WriteLine(NekoStringToParse);
            Enum.TryParse(NekoStringToParse, out NSFWNekos neko);
            return neko;
        }

        #endregion

        #region Gifs
        /// <summary>
        /// Gets an animated NSFW image, uses specified input text if any.
        /// </summary>
        /// <param name="text">Input text</param>
        /// <returns>Lewd animations to your discord channel</returns>
        [Command("NSFWGIF")]
        [RequireNsfw]
        public async Task NSFWGIFAsync([Remainder] string text = "")
        {
            AnimatedNSFWNekos res;

            if (text == "")
                res = Utilities.Enum.Enum.RandomEnumValue<AnimatedNSFWNekos>();
            else
                res = GetNSFWGifFromString(text);

            var s = await _module.GetStreamFromEnumAsync(res);
            await Context.Channel.SendFileAsync(s, "OwO.gif");
        }


        /// <summary>
        /// Attempts to get the relevant NSFW GIF enum from a string
        /// </summary>
        /// <param name="nekoString">Input name</param>
        /// <returns>AnimatedNSFWNekos</returns>
        private AnimatedNSFWNekos GetNSFWGifFromString(string nekoString)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            string NekoStringToParse = textInfo.ToTitleCase(nekoString.Split(' ').FirstOrDefault());
            Console.WriteLine(NekoStringToParse);
            Enum.TryParse(NekoStringToParse, out AnimatedNSFWNekos neko);
            return neko;
        }
        #endregion

        #endregion

        #region Action Nekos

        [Command("Cuddle")]
        public async Task CuddleAsync()
        {
            var s = await _module.GetStreamFromEnumAsync(ActionNekos.Cuddlegif);
            await Context.Channel.SendFileAsync(s, "Cuffle.gif");
        }

        [Command("Feed")]
        public async Task FeedAsync()
        {
            var s = await _module.GetStreamFromEnumAsync(ActionNekos.Feedgif);
            await Context.Channel.SendFileAsync(s, "Feed.gif");
        }

        [Command("Hug")]
        public async Task HugAsync()
        {
            var s = await _module.GetStreamFromEnumAsync(ActionNekos.Huggif);
            await Context.Channel.SendFileAsync(s, "Hug.gif");
        }

        [Command("Kiss")]
        public async Task KissAsync()
        {
            var s = await _module.GetStreamFromEnumAsync(ActionNekos.Kissgif);
            await Context.Channel.SendFileAsync(s, "Kiss.gif");
        }

        [Command("Pat")]
        public async Task PatAsync()
        {
            var s = await _module.GetStreamFromEnumAsync(ActionNekos.Patgif);
            await Context.Channel.SendFileAsync(s, "Pat.gif");
        }

        [Command("Poke")]
        public async Task PokeAsync()
        {
            var s = await _module.GetStreamFromEnumAsync(ActionNekos.Pokegif);
            await Context.Channel.SendFileAsync(s, "Poke.gif");
        }

        [Command("Slap")]
        public async Task SlapAsync()
        {
            var s = await _module.GetStreamFromEnumAsync(ActionNekos.Slapgif);
            await Context.Channel.SendFileAsync(s, "Slap.gif");
        }

        [Command("Tickle")]
        public async Task TickleAsync()
        {
            var s = await _module.GetStreamFromEnumAsync(ActionNekos.Ticklegif);
            await Context.Channel.SendFileAsync(s, "Tickle.gif");
        }

        #endregion
    }

}