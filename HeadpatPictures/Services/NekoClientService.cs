using HeadpatPictures.Contracts;

using System;
using NekosSharp;
using System.Threading.Tasks;
using System.IO;
using Enums.PictureServices;
using System.Net.Http;

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

            //bitmap = WriteOnBitmap(bitmap, Text, Size);
            Console.WriteLine(Req.ImageUrl);
            return s;
        }

        private void CheckForCachedPictures()
        {

        }
    }
}
