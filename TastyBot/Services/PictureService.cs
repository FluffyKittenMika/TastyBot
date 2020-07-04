using TastyBot.Contracts;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using NekosSharp;
using System.Drawing;
using System;
using System.Drawing.Imaging;
using Enums.PictureServices;
using System.ComponentModel;
using System.Linq;

namespace TastyBot.Services
{
    public class PictureService : IPictureService
    {
        private readonly HttpClient _http;
        public static NekoClient NekoClient;

        public PictureService()
        {
            _http = new HttpClient();
            NekoClient = new NekoClient("TastyBot");
        }

        public async Task<Stream> GetCatGifAsync()
        {
            var resp = await _http.GetAsync("https://cataas.com/cat/gif");
            return await resp.Content.ReadAsStreamAsync();
        }
        public async Task<Stream> GetCatPictureAsync(string Text = " ", string Color = "white", int Size = 32)
        {
            var resp = await _http.GetAsync($"https://cataas.com/cat/says/" + Text + "?size=" + Size + "&color=" + Color);
            return await resp.Content.ReadAsStreamAsync();
        }

        /// <summary>
        /// Gets picture from NekoClient based on the given type.
        /// Appends text to the image if there's any given.
        /// First words can be treated as a color.
        /// </summary>
        /// <param name="Types"></param>
        /// <param name="Text"></param>
        /// <returns></returns>
        public async Task<Stream> GetRegularNekoClientPictureAsync(RegularNekos Types, string Text = "")
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
                s = WriteOnStream(s, Text);

            //bitmap = WriteOnBitmap(bitmap, Text, Size);
            Console.WriteLine(Req.ImageUrl);
            s.Seek(0, SeekOrigin.Begin);
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
                s = WriteOnStream(s, Text);

            //bitmap = WriteOnBitmap(bitmap, Text, Size);
            Console.WriteLine(Req.ImageUrl);
            s.Seek(0, SeekOrigin.Begin);
            return s;
        }


        /// <summary>
        /// Gets the color of a string.
        /// Only uses the first word.
        /// </summary>          
        /// <param name="check">Input string to check, if there's a color it also shortens the string to not include it</param>
        /// <returns>Correct color, OR black if none is found</returns>
        private Color GetColor(ref string check)
        {
            string Col = check.Split(' ').FirstOrDefault();
            Color color = Color.FromName(Col);
            if (!color.IsKnownColor) //checks if known
                color = Color.FromName("black"); //if not, black it is!
            else
                check = string.Join(' ', check.Split(' ').Skip(1));
            return color;
        }


        //TODO: Text wrapping to fit more text into the fucking thing
        //TODO: Enable rainbow text
        /// <summary>
        /// Writes text on a given imagestream.
        /// </summary>
        /// <param name="stream">Image Stream</param>
        /// <param name="text">Text for the image</param>
        /// <returns>PNG formatted memorystream</returns>
        private Stream WriteOnStream(Stream stream, string text = "")
        {
            Bitmap bitmap = new Bitmap(stream);
            Color color = GetColor(ref text);

            //Make bounds
            Rectangle rect = new Rectangle(0, 0, bitmap.Width, (int)(bitmap.Height * 1.50)); //puts it to half bottom 
            //Holy hell i hate appending text to a bitmap
            //Define font, 'n alignment
            Font ffont = new Font("Tahoma", 32);
            StringFormat stringFormat = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center,
                Trimming = StringTrimming.EllipsisWord
            };

            //Define the god damn graphics
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                SizeF s = g.MeasureString(text, ffont);
                float fontScale = Math.Max(s.Width / rect.Width, s.Height / rect.Height);
                using (Font font = new Font(ffont.FontFamily, ffont.SizeInPoints / fontScale, GraphicsUnit.Point)) //probably don't need to redefine font here
                {
                    g.DrawString(text, font, new SolidBrush(color), rect, stringFormat);
                }
                g.Flush();
            }
            MemoryStream resultstream = new MemoryStream();
            bitmap.Save(resultstream, ImageFormat.Png);

            return resultstream;
        }
    }
}
