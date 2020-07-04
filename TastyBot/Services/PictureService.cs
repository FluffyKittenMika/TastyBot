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
        private readonly HttpClient _http = new HttpClient();
        public static NekoClient NekoClient = new NekoClient("TastyBot");

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
                check = check.Substring(check.IndexOf(' ') + 1); // skips first word
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
