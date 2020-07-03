using TastyBot.Contracts;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using NekosSharp;
using System.Drawing;
using System;
using System.Drawing.Imaging;

namespace TastyBot.Services
{
    public class PictureService : IPictureService
    {
        private readonly HttpClient _http;
        public static NekoClient NekoClient = new NekoClient("TastyBot");

        public PictureService(HttpClient http)
            => _http = http;

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

        public async Task<Stream> GetNekoPictureAsync(string Text = " ", string Color = "black")
        {
            //Request the neko url
            Request Req = await NekoClient.Image_v3.Neko();

            //process it into a bitmap
            var resp = await _http.GetAsync(Req.ImageUrl);

            Stream s = await resp.Content.ReadAsStreamAsync();

            s = WriteOnStream(s, Text, Color);

            //bitmap = WriteOnBitmap(bitmap, Text, Size);
            Console.WriteLine(Req.ImageUrl);
            return s;
        }
       

        //TODO: make async, i haven't the clue go'vna on how, not that it works yet :)
        //TODO: Text wrapping to fit more text into the fucking thing
        public Stream WriteOnStream(Stream stream, string text = " ", string col = "black")
        {
            Bitmap bitmap = new Bitmap(stream);
            Color color = Color.FromName(col);
            if (!color.IsKnownColor) //prevents it from going transparent if there's a bullshit color given, looking at you realitycat
                color = Color.FromName("white");

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
