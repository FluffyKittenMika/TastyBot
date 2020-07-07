using HeadpatPictures.Contracts;

using System;
using NekosSharp;
using System.Threading.Tasks;
using System.IO;
using Enums.PictureServices;
using System.Net.Http;
using Utilities.LoggingService;

namespace HeadpatPictures.Services
{
    public class NekoClientService : INekoClientService
    {
        private readonly NekoClient NekoClient;
        private readonly HttpClient _http;
        private readonly ITextStreamWriter _writer;

        public NekoClientService(ITextStreamWriter writer)
        {
            NekoClient = new NekoClient("TastyBot");
            _http = new HttpClient();
            _writer = writer;
        }


        /// <summary>
        /// Gets an action gif.
        /// Does not support writing to the Gif
        /// </summary>
        /// <param name="action">The wanted action</param>
        /// <returns>Stream of a Gif</returns>
        public async Task<Stream> GetActionNekoClientGifAsync(ActionNekos Types)
        {
            var Req = Types switch
            {
                ActionNekos.Cuddlegif => await NekoClient.Action_v3.CuddleGif(),
                ActionNekos.Feedgif => await NekoClient.Action_v3.FeedGif(),
                ActionNekos.Huggif => await NekoClient.Action_v3.HugGif(),
                ActionNekos.Kissgif => await NekoClient.Action_v3.KissGif(),
                ActionNekos.Patgif => await NekoClient.Action_v3.PatGif(),
                ActionNekos.Pokegif => await NekoClient.Action_v3.PokeGif(),
                ActionNekos.Slapgif => await NekoClient.Action_v3.SlapGif(),
                ActionNekos.Ticklegif => await NekoClient.Action_v3.TickleGif(),
                _ => await NekoClient.Action_v3.PatGif(),
            };

            //process it into a bitmap
            var resp = await _http.GetAsync(Req.ImageUrl);

            //Fetch the goodies
            Stream s = await resp.Content.ReadAsStreamAsync();

            await Logging.LogInfoMessage("ActionNekos", Req.ImageUrl);
            return s;
        }


        /// <summary>
        /// Gets picture from NekoClient based on the given type.
        /// Appends text to the image if there's any given.
        /// First words can be treated as a color.
        /// </summary>
        /// <param name="Types"></param>
        /// <param name="Text"></param>
        /// <returns></returns>
        public async Task<Stream> GetSFWNekoClientPictureAsync(RegularNekos Types, string Text = "")
        {
            //Request the neko url
            //Fuck you Earan, this is the new switch statement.
            var Req = Types switch
            {
                RegularNekos.Avatar => await NekoClient.Image_v3.Avatar(),
                RegularNekos.Fox => await NekoClient.Image_v3.Fox(),
                RegularNekos.Holo => await NekoClient.Image_v3.Holo(),
                RegularNekos.Neko => await NekoClient.Image_v3.Neko(),
                RegularNekos.NekoAvatar => await NekoClient.Image_v3.NekoAvatar(),
                RegularNekos.Waifu => await NekoClient.Image_v3.Waifu(),
                RegularNekos.Wallpaper => await NekoClient.Image_v3.Wallpaper(),
                _ => await NekoClient.Image_v3.Neko(),
            };

            //process it into a bitmap
            var resp = await _http.GetAsync(Req.ImageUrl);

            //Fetch the goodies
            Stream s = await resp.Content.ReadAsStreamAsync();

            //only write if they want us to
            if (Text != "")
                s = _writer.WriteOnStream(s, Text);

            await Logging.LogInfoMessage("RegularNekos", Req.ImageUrl);
            return s;
        }

        /// <summary>
        /// Gets picture from NekoClient based on the given type.
        /// Appends text to the image if there's any given.
        /// First words can be treated as a color.
        /// </summary>
        /// <param name="Types"></param>
        /// <param name="Text"></param>
        /// <returns></returns>
        public async Task<Stream> GetNSFWNekoClientPictureAsync(NSFWNekos Types, string Text = "")
        {
            //Request the neko url
            //I hate this work - Mik
            var Req = Types switch
            {
                NSFWNekos.Ahegao => await NekoClient.Nsfw_v3.Ahegao(),
                NSFWNekos.Anal => await NekoClient.Nsfw_v3.Anal(),
                NSFWNekos.Anus => await NekoClient.Nsfw_v3.Anus(),
                NSFWNekos.BDSM => await NekoClient.Nsfw_v3.Bdsm(),
                NSFWNekos.Blowjob => await NekoClient.Nsfw_v3.Blowjob(),
                NSFWNekos.Boobs => await NekoClient.Nsfw_v3.Boobs(),
                NSFWNekos.Classic => await NekoClient.Nsfw_v3.Classic(),
                NSFWNekos.Cosplay => await NekoClient.Nsfw_v3.Cosplay(),
                NSFWNekos.Cum => await NekoClient.Nsfw_v3.Cum(),
                NSFWNekos.Erofeet => await NekoClient.Nsfw_v3.EroFeet(),
                NSFWNekos.Erofox => await NekoClient.Nsfw_v3.EroFox(),
                NSFWNekos.Erofox2 => await NekoClient.Nsfw_v3.EroFox2(),
                NSFWNekos.Eroholo => await NekoClient.Nsfw_v3.EroHolo(),
                NSFWNekos.Eroneko => await NekoClient.Nsfw_v3.EroNeko(),
                NSFWNekos.Eropantyhose => await NekoClient.Nsfw_v3.EroPantyhose(),
                NSFWNekos.Eropiersing => await NekoClient.Nsfw_v3.EroPiersing(),
                NSFWNekos.Erowallpaper => await NekoClient.Nsfw_v3.EroWallpaper(),
                NSFWNekos.Eroyuri => await NekoClient.Nsfw_v3.EroYuri(),
                NSFWNekos.Feet => await NekoClient.Nsfw_v3.Feet(),
                NSFWNekos.Femdom => await NekoClient.Nsfw_v3.Femdom(),
                NSFWNekos.Fox => await NekoClient.Nsfw_v3.Fox(),
                NSFWNekos.Fox2 => await NekoClient.Nsfw_v3.Fox2(),
                NSFWNekos.Futanari => await NekoClient.Nsfw_v3.Futanari(),
                NSFWNekos.Holo => await NekoClient.Nsfw_v3.Holo(),
                NSFWNekos.Holoavatar => await NekoClient.Nsfw_v3.HoloAvatar(),
                NSFWNekos.Keta => await NekoClient.Nsfw_v3.Keta(),
                NSFWNekos.Ketaavatar => await NekoClient.Nsfw_v3.KetaAvatar(),
                NSFWNekos.Lewd => await NekoClient.Nsfw_v3.Lewd(),
                NSFWNekos.Neko => await NekoClient.Nsfw_v3.Neko(),
                NSFWNekos.Pantyhose => await NekoClient.Nsfw_v3.Pantyhose(),
                NSFWNekos.Peeing => await NekoClient.Nsfw_v3.Peeing(),
                NSFWNekos.Piersing => await NekoClient.Nsfw_v3.Piersing(),
                NSFWNekos.Pussy => await NekoClient.Nsfw_v3.Pussy(),
                NSFWNekos.Smallboobs => await NekoClient.Nsfw_v3.SmallBoobs(),
                NSFWNekos.Solo => await NekoClient.Nsfw_v3.Solo(),
                NSFWNekos.Trap => await NekoClient.Nsfw_v3.Trap(),
                NSFWNekos.Wallpaper => await NekoClient.Nsfw_v3.Wallpaper(),
                NSFWNekos.Yiff => await NekoClient.Nsfw_v3.Yiff(),
                NSFWNekos.Yuri => await NekoClient.Nsfw_v3.Yuri(),
                _ => await NekoClient.Nsfw_v3.Yuri(),
            };

            //process it into a bitmap
            var resp = await _http.GetAsync(Req.ImageUrl);

            //Fetch the goodies
            Stream s = await resp.Content.ReadAsStreamAsync();

            //only write if they want us to
            if (Text != "")
                s = _writer.WriteOnStream(s, Text);

            await Logging.LogInfoMessage("NSFWNekos", Req.ImageUrl);
            return s;
        }

        public async Task<Stream> NSFWNekoClientGifAsync(AnimatedNSFWNekos actionNekos)
        {
            var Req = actionNekos switch
            {
                AnimatedNSFWNekos.Analgif => await NekoClient.Nsfw_v3.AnalGif(),
                AnimatedNSFWNekos.BlowJobgif => await NekoClient.Nsfw_v3.BlowjobGif(),
                AnimatedNSFWNekos.Boobsgif => await NekoClient.Nsfw_v3.BoobsGif(),
                AnimatedNSFWNekos.Classicgif => await NekoClient.Nsfw_v3.ClassicGif(),
                AnimatedNSFWNekos.Cumgif => await NekoClient.Nsfw_v3.CumGif(),
                AnimatedNSFWNekos.Feetgif => await NekoClient.Nsfw_v3.FeetGif(),
                AnimatedNSFWNekos.Hentaigif => await NekoClient.Nsfw_v3.HentaiGif(),
                AnimatedNSFWNekos.Kunigif => await NekoClient.Nsfw_v3.KuniGif(),
                AnimatedNSFWNekos.Nekogif => await NekoClient.Nsfw_v3.NekoGif(),
                AnimatedNSFWNekos.Pussygif => await NekoClient.Nsfw_v3.PussyGif(),
                AnimatedNSFWNekos.Pwankgif => await NekoClient.Nsfw_v3.PwankGif(),
                AnimatedNSFWNekos.Sologif => await NekoClient.Nsfw_v3.SoloGif(),
                AnimatedNSFWNekos.Spankgif => await NekoClient.Nsfw_v3.SpankGif(),
                AnimatedNSFWNekos.Yiffgif => await NekoClient.Nsfw_v3.YiffGif(),
                AnimatedNSFWNekos.Yurigif => await NekoClient.Nsfw_v3.YuriGif(),
                _ => await NekoClient.Nsfw_v3.BoobsGif(),
            };

            //process it into a bitmap
            var resp = await _http.GetAsync(Req.ImageUrl);

            //Fetch the goodies
            Stream s = await resp.Content.ReadAsStreamAsync();

            await Logging.LogInfoMessage("AnimatedNSFWNekos", Req.ImageUrl);
            return s;
        }

        public async Task<Stream> SFWNekoClientGifAsync(AnimatedNekos actionNekos)
        {
            var Req = actionNekos switch
            {
                AnimatedNekos.Bakagif => await NekoClient.Image_v3.BakaGif(),
                AnimatedNekos.Nekogif => await NekoClient.Image_v3.NekoGif(),
                AnimatedNekos.Smuggif => await NekoClient.Image_v3.SmugGif(),
                _ => await NekoClient.Image_v3.BakaGif(),
            };

            //process it into a bitmap
            var resp = await _http.GetAsync(Req.ImageUrl);

            //Fetch the goodies
            Stream s = await resp.Content.ReadAsStreamAsync();

            await Logging.LogInfoMessage("AnimatedSFWNekos", Req.ImageUrl);
            return s;
        }
    }
}
