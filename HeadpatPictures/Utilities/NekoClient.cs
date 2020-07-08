using NekosSharp;
using System.Threading.Tasks;
using System.IO;
using Enums.PictureServices;
using System.Net.Http;
using Utilities.LoggingService;
using System;

namespace HeadpatPictures.Utilities
{
    public static class NekoClient
    {
        private static readonly NekosSharp.NekoClient _nekoClient = new NekosSharp.NekoClient("TastyBot");
        private static readonly HttpClient _http = new HttpClient();

        #region ActionNeko

        /// <summary>
        /// Gets an action gif.
        /// Does not support writing to the Gif
        /// </summary>
        /// <param name="action">The wanted action</param>
        /// <returns>Stream of a Gif</returns>
        private static async Task<Request> GetActionNekoClientGifAsync(ActionNekos Types)
        {
            return Types switch
            {
                ActionNekos.Cuddlegif => await _nekoClient.Action_v3.CuddleGif(),
                ActionNekos.Feedgif => await _nekoClient.Action_v3.FeedGif(),
                ActionNekos.Huggif => await _nekoClient.Action_v3.HugGif(),
                ActionNekos.Kissgif => await _nekoClient.Action_v3.KissGif(),
                ActionNekos.Patgif => await _nekoClient.Action_v3.PatGif(),
                ActionNekos.Pokegif => await _nekoClient.Action_v3.PokeGif(),
                ActionNekos.Slapgif => await _nekoClient.Action_v3.SlapGif(),
                ActionNekos.Ticklegif => await _nekoClient.Action_v3.TickleGif(),
                _ => await _nekoClient.Action_v3.PatGif(),
            };
        }

        #endregion

        #region SFW

        /// <summary>
        /// Gets picture from NekoClient based on the given type.
        /// Appends text to the image if there's any given.
        /// First words can be treated as a color.
        /// </summary>
        /// <param name="Types"></param>
        /// <param name="Text"></param>
        /// <returns></returns>
        private static async Task<Request> GetSFWNekoClientPictureAsync(RegularNekos Types)
        {
            return Types switch
            {
                RegularNekos.Avatar => await _nekoClient.Image_v3.Avatar(),
                RegularNekos.Fox => await _nekoClient.Image_v3.Fox(),
                RegularNekos.Holo => await _nekoClient.Image_v3.Holo(),
                RegularNekos.Neko => await _nekoClient.Image_v3.Neko(),
                RegularNekos.NekoAvatar => await _nekoClient.Image_v3.NekoAvatar(),
                RegularNekos.Waifu => await _nekoClient.Image_v3.Waifu(),
                RegularNekos.Wallpaper => await _nekoClient.Image_v3.Wallpaper(),
                _ => await _nekoClient.Image_v3.Neko(),
            };
        }

        private static async Task<Request> SFWNekoClientGifAsync(AnimatedNekos actionNekos)
        {
            return actionNekos switch
            {
                AnimatedNekos.Bakagif => await _nekoClient.Image_v3.BakaGif(),
                AnimatedNekos.Nekogif => await _nekoClient.Image_v3.NekoGif(),
                AnimatedNekos.Smuggif => await _nekoClient.Image_v3.SmugGif(),
                _ => await _nekoClient.Image_v3.BakaGif(),
            };
        }

        #endregion

        #region NSFW

        /// <summary>
        /// Gets picture from NekoClient based on the given type.
        /// Appends text to the image if there's any given.
        /// First words can be treated as a color.
        /// </summary>
        /// <param name="Types"></param>
        /// <param name="Text"></param>
        /// <returns></returns>
        private static async Task<Request> GetNSFWNekoClientPictureAsync(NSFWNekos Types)
        {
            //Request the neko url
            //I hate this work - Mik
            return Types switch
            {
                NSFWNekos.Ahegao => await _nekoClient.Nsfw_v3.Ahegao(),
                NSFWNekos.Anal => await _nekoClient.Nsfw_v3.Anal(),
                NSFWNekos.Anus => await _nekoClient.Nsfw_v3.Anus(),
                NSFWNekos.BDSM => await _nekoClient.Nsfw_v3.Bdsm(),
                NSFWNekos.Blowjob => await _nekoClient.Nsfw_v3.Blowjob(),
                NSFWNekos.Boobs => await _nekoClient.Nsfw_v3.Boobs(),
                NSFWNekos.Classic => await _nekoClient.Nsfw_v3.Classic(),
                NSFWNekos.Cosplay => await _nekoClient.Nsfw_v3.Cosplay(),
                NSFWNekos.Cum => await _nekoClient.Nsfw_v3.Cum(),
                NSFWNekos.Erofeet => await _nekoClient.Nsfw_v3.EroFeet(),
                NSFWNekos.Erofox => await _nekoClient.Nsfw_v3.EroFox(),
                NSFWNekos.Erofox2 => await _nekoClient.Nsfw_v3.EroFox2(),
                NSFWNekos.Eroholo => await _nekoClient.Nsfw_v3.EroHolo(),
                NSFWNekos.Eroneko => await _nekoClient.Nsfw_v3.EroNeko(),
                NSFWNekos.Eropantyhose => await _nekoClient.Nsfw_v3.EroPantyhose(),
                NSFWNekos.Eropiersing => await _nekoClient.Nsfw_v3.EroPiersing(),
                NSFWNekos.Erowallpaper => await _nekoClient.Nsfw_v3.EroWallpaper(),
                NSFWNekos.Eroyuri => await _nekoClient.Nsfw_v3.EroYuri(),
                NSFWNekos.Feet => await _nekoClient.Nsfw_v3.Feet(),
                NSFWNekos.Femdom => await _nekoClient.Nsfw_v3.Femdom(),
                NSFWNekos.Fox => await _nekoClient.Nsfw_v3.Fox(),
                NSFWNekos.Fox2 => await _nekoClient.Nsfw_v3.Fox2(),
                NSFWNekos.Futanari => await _nekoClient.Nsfw_v3.Futanari(),
                NSFWNekos.Holo => await _nekoClient.Nsfw_v3.Holo(),
                NSFWNekos.Holoavatar => await _nekoClient.Nsfw_v3.HoloAvatar(),
                NSFWNekos.Keta => await _nekoClient.Nsfw_v3.Keta(),
                NSFWNekos.Ketaavatar => await _nekoClient.Nsfw_v3.KetaAvatar(),
                NSFWNekos.Lewd => await _nekoClient.Nsfw_v3.Lewd(),
                NSFWNekos.Neko => await _nekoClient.Nsfw_v3.Neko(),
                NSFWNekos.Pantyhose => await _nekoClient.Nsfw_v3.Pantyhose(),
                NSFWNekos.Peeing => await _nekoClient.Nsfw_v3.Peeing(),
                NSFWNekos.Piersing => await _nekoClient.Nsfw_v3.Piersing(),
                NSFWNekos.Pussy => await _nekoClient.Nsfw_v3.Pussy(),
                NSFWNekos.Smallboobs => await _nekoClient.Nsfw_v3.SmallBoobs(),
                NSFWNekos.Solo => await _nekoClient.Nsfw_v3.Solo(),
                NSFWNekos.Trap => await _nekoClient.Nsfw_v3.Trap(),
                NSFWNekos.Wallpaper => await _nekoClient.Nsfw_v3.Wallpaper(),
                NSFWNekos.Yiff => await _nekoClient.Nsfw_v3.Yiff(),
                NSFWNekos.Yuri => await _nekoClient.Nsfw_v3.Yuri(),
                _ => await _nekoClient.Nsfw_v3.Yuri(),
            };
        }

        private static async Task<Request> NSFWNekoClientGifAsync(AnimatedNSFWNekos actionNekos)
        {
            return actionNekos switch
            {
                AnimatedNSFWNekos.Analgif => await _nekoClient.Nsfw_v3.AnalGif(),
                AnimatedNSFWNekos.BlowJobgif => await _nekoClient.Nsfw_v3.BlowjobGif(),
                AnimatedNSFWNekos.Boobsgif => await _nekoClient.Nsfw_v3.BoobsGif(),
                AnimatedNSFWNekos.Classicgif => await _nekoClient.Nsfw_v3.ClassicGif(),
                AnimatedNSFWNekos.Cumgif => await _nekoClient.Nsfw_v3.CumGif(),
                AnimatedNSFWNekos.Feetgif => await _nekoClient.Nsfw_v3.FeetGif(),
                AnimatedNSFWNekos.Hentaigif => await _nekoClient.Nsfw_v3.HentaiGif(),
                AnimatedNSFWNekos.Kunigif => await _nekoClient.Nsfw_v3.KuniGif(),
                AnimatedNSFWNekos.Nekogif => await _nekoClient.Nsfw_v3.NekoGif(),
                AnimatedNSFWNekos.Pussygif => await _nekoClient.Nsfw_v3.PussyGif(),
                AnimatedNSFWNekos.Pwankgif => await _nekoClient.Nsfw_v3.PwankGif(),
                AnimatedNSFWNekos.Sologif => await _nekoClient.Nsfw_v3.SoloGif(),
                AnimatedNSFWNekos.Spankgif => await _nekoClient.Nsfw_v3.SpankGif(),
                AnimatedNSFWNekos.Yiffgif => await _nekoClient.Nsfw_v3.YiffGif(),
                AnimatedNSFWNekos.Yurigif => await _nekoClient.Nsfw_v3.YuriGif(),
                _ => await _nekoClient.Nsfw_v3.BoobsGif(),
            };
        }

        #endregion

        /// <summary>
        /// Gets a picture or a gif depending on the given pictureType and enum
        /// </summary>
        /// <param name="pictureType">The type of picture you want</param>
        /// <param name="nekoEnum">The subcatogory of pictures from that type</param>
        /// <returns>
        /// Stream of either a gif or a picture
        /// </returns>
        public static async Task<Stream> GetNekoClientItem<T>(PictureTypes pictureType, T nekoEnum)
        {
            Request req = pictureType switch
            {
                PictureTypes.ActionNekos => await GetActionNekoClientGifAsync(Enum.Parse<ActionNekos>(nekoEnum.ToString())),
                PictureTypes.AnimatedNekos => await SFWNekoClientGifAsync(Enum.Parse<AnimatedNekos>(nekoEnum.ToString())),
                PictureTypes.AnimatedNSFWNekos => await NSFWNekoClientGifAsync(Enum.Parse<AnimatedNSFWNekos>(nekoEnum.ToString())),
                PictureTypes.NSFWNekos => await GetNSFWNekoClientPictureAsync(Enum.Parse<NSFWNekos>(nekoEnum.ToString())),
                PictureTypes.RegularNekos => await GetSFWNekoClientPictureAsync(Enum.Parse<RegularNekos>(nekoEnum.ToString())),
                _ => null,
            };

            if (req == null)
            {
                await Logging.LogDebugMessage($"Neko", $"Default PictureType triggered, Please add this new pictureType to the switch.");
                throw new NotImplementedException();
            }

            //process it into a bitmap
            var resp = await _http.GetAsync(req.ImageUrl);

            //Fetch the goodies
            Stream s = await resp.Content.ReadAsStreamAsync();

            await Logging.LogInfoMessage(pictureType.ToString(), req.ImageUrl);
            return s;
        }
    }
}
