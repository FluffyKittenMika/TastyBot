using Discord.Commands;
using System.Threading.Tasks;
using Enums.PictureServices;
using System;
using System.Linq;
using System.Globalization;
using System.IO;
using Utilities.PictureUtilities;
using System.Collections.Generic;
using DiscordUI.Contracts;

namespace DiscordUI.Modules
{
    [Name("Picture Commands")]
    public class PictureModule : ModuleBase<SocketCommandContext>
    {
        private readonly IPictureCacheService _pic;

        public PictureModule(IPictureCacheService pic)
        {
            _pic = pic;
        }

        #region Cat meow meow region

        [Command("cat")]
        public async Task CatAsync([Remainder] string text = "")
        {
            Stream stream = await _pic.ReturnFastestStream(Cache.RetrieveItems<List<Stream>>, Cache.StoreItems, Cache.CacheExists, "Cat");
            stream = TextStreamWriter.WriteOnStream(stream, text);
            var image = await Context.Channel.SendFileAsync(stream, "cat.png");
        }

        [Command("catgif")]
        public async Task CatGifAsync()
        {
            Stream stream = await _pic.ReturnFastestStream(Cache.RetrieveItems<List<Stream>>, Cache.StoreItems, Cache.CacheExists, "CatGif");
            await Context.Channel.SendFileAsync(stream, "cat.gif");
        }

        #endregion 

        #region Nekos

        [Command("neko")]
        public async Task NekoAsync([Remainder] string text = "")
        {
            Stream stream = await _pic.ReturnFastestStream(Cache.RetrieveItems<List<Stream>>, Cache.StoreItems, Cache.CacheExists, SFWNekos.Neko);
            stream = TextStreamWriter.WriteOnStream(stream, text);            
            await Context.Channel.SendFileAsync(stream, "Neko.png");
        }

        [Command("nekoavatar")]
        public async Task NekoAvatarAsync([Remainder] string text = "")
        {
            Stream stream = await _pic.ReturnFastestStream(Cache.RetrieveItems<List<Stream>>, Cache.StoreItems, Cache.CacheExists, SFWNekos.Avatar);
            stream = TextStreamWriter.WriteOnStream(stream, text);            
            await Context.Channel.SendFileAsync(stream, "Avatar.png");
        }

        [Command("nekowallpaper")]
        public async Task NekoWallpaperAsync([Remainder] string text = "")
        {
            Stream stream = await _pic.ReturnFastestStream(Cache.RetrieveItems<List<Stream>>, Cache.StoreItems, Cache.CacheExists, SFWNekos.Wallpaper);
            stream = TextStreamWriter.WriteOnStream(stream, text);    
            await Context.Channel.SendFileAsync(stream, "Wallpaper.png");
        }

        [Command("fox")]
        public async Task FoxAsync([Remainder] string text = "")
        {
            Stream stream = await _pic.ReturnFastestStream(Cache.RetrieveItems<List<Stream>>, Cache.StoreItems, Cache.CacheExists, SFWNekos.Fox);
            stream = TextStreamWriter.WriteOnStream(stream, text);            
            await Context.Channel.SendFileAsync(stream, "Fox.png");
        }

        [Command("waifu")]
        public async Task WaifuAsync([Remainder] string text = "")
        {
            Stream stream = await _pic.ReturnFastestStream(Cache.RetrieveItems<List<Stream>>, Cache.StoreItems, Cache.CacheExists, SFWNekos.Waifu);
            stream = TextStreamWriter.WriteOnStream(stream, text);
            await Context.Channel.SendFileAsync(stream, "Waifu.png");
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
        public async Task NSFWAsync([Remainder] string text = "")
        {
            NSFWNekos res;

            if (text == "")
                res = Utilities.Enum.Enum.RandomEnumValue<NSFWNekos>();
            else
                res = GetNSFWNekoFromString(text);

            text = string.Join(' ', text.Split(' ').Skip(1));
            Stream stream = await _pic.ReturnFastestStream(Cache.RetrieveItems<List<Stream>>, Cache.StoreItems, Cache.CacheExists, res);
            stream = TextStreamWriter.WriteOnStream(stream, text);            
            await Context.Channel.SendFileAsync(stream, "OwO.png");
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
            if (!Enum.TryParse(NekoStringToParse, out NSFWNekos neko))
            {
                neko = Utilities.Enum.Enum.RandomEnumValue<NSFWNekos>();
            }
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

            Stream stream = await _pic.ReturnFastestStream(Cache.RetrieveItems<List<Stream>>, Cache.StoreItems, Cache.CacheExists, res);
            await Context.Channel.SendFileAsync(stream, "OwO.gif");
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
            if (!Enum.TryParse(NekoStringToParse, out AnimatedNSFWNekos neko))
            {
                neko = Utilities.Enum.Enum.RandomEnumValue<AnimatedNSFWNekos>();
            }
            return neko;
        }
        #endregion

        #endregion

        #region Action Nekos

        [Command("Cuddle")]
        public async Task CuddleAsync()
        {
            Stream stream = await _pic.ReturnFastestStream(Cache.RetrieveItems<List<Stream>>, Cache.StoreItems, Cache.CacheExists, ActionNekos.Cuddlegif);
            await Context.Channel.SendFileAsync(stream, "Cuffle.gif");
        }

        [Command("Feed")]
        public async Task FeedAsync()
        {
            Stream stream = await _pic.ReturnFastestStream(Cache.RetrieveItems<List<Stream>>, Cache.StoreItems, Cache.CacheExists, ActionNekos.Feedgif);
            await Context.Channel.SendFileAsync(stream, "Feed.gif");
        }

        [Command("Hug")]
        public async Task HugAsync()
        {
            Stream stream = await _pic.ReturnFastestStream(Cache.RetrieveItems<List<Stream>>, Cache.StoreItems, Cache.CacheExists, ActionNekos.Huggif);
            await Context.Channel.SendFileAsync(stream, "Hug.gif");
        }

        [Command("Kiss")]
        public async Task KissAsync()
        {
            Stream stream = await _pic.ReturnFastestStream(Cache.RetrieveItems<List<Stream>>, Cache.StoreItems, Cache.CacheExists, ActionNekos.Kissgif);
            await Context.Channel.SendFileAsync(stream, "Kiss.gif");
        }

        [Command("Pat")]
        public async Task PatAsync()
        {
            Stream stream = await _pic.ReturnFastestStream(Cache.RetrieveItems<List<Stream>>, Cache.StoreItems, Cache.CacheExists, ActionNekos.Patgif);
            await Context.Channel.SendFileAsync(stream, "Pat.gif");
        }

        [Command("Poke")]
        public async Task PokeAsync()
        {
            Stream stream = await _pic.ReturnFastestStream(Cache.RetrieveItems<List<Stream>>, Cache.StoreItems, Cache.CacheExists, ActionNekos.Pokegif);
            await Context.Channel.SendFileAsync(stream, "Poke.gif");
        }

        [Command("Slap")]
        public async Task SlapAsync()
        {
            Stream stream = await _pic.ReturnFastestStream(Cache.RetrieveItems<List<Stream>>, Cache.StoreItems, Cache.CacheExists, ActionNekos.Slapgif);
            await Context.Channel.SendFileAsync(stream, "Slap.gif");
        }

        [Command("Tickle")]
        public async Task TickleAsync()
        {
            Stream stream = await _pic.ReturnFastestStream(Cache.RetrieveItems<List<Stream>>, Cache.StoreItems, Cache.CacheExists, ActionNekos.Ticklegif);
            await Context.Channel.SendFileAsync(stream, "Tickle.gif");
        }

        #endregion
    }

}