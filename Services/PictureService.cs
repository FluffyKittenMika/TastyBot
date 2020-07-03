using Newtonsoft.Json.Linq;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using NekosSharp;
using System.Drawing;
using System;

namespace TastyBot.Services
{
    public class PictureService
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

        public async Task<Bitmap> GetNekoPictureAsync(string Text = " ", int Size = 32)
        {
            //Request the neko url
            Request Req = await NekoClient.Image_v3.Neko();

            //process it into a bitmap
            var resp = await _http.GetAsync(Req.ImageUrl);
            var bitmap = new Bitmap(await resp.Content.ReadAsStreamAsync());

            bitmap = WriteOnBitmap(bitmap, Text, Size);

            return bitmap;
        }

        //TODO: make async, i haven't the clue go'vna on how
        public Bitmap WriteOnBitmap(Bitmap bitmap, string text = " ", int Size = 32)
        {
            //Make bounds
            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            //Holy hell i hate appending text to a bitmap
            //Define font, 'n alignment
            Font ffont = new Font("Tahoma", Size);
            StringFormat stringFormat = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            //Define the god damn graphics
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                SizeF s = g.MeasureString(text, ffont);
                float fontScale = Math.Max(s.Width / rect.Width, s.Height / rect.Height);
                using (Font font = new Font(ffont.FontFamily, ffont.SizeInPoints / fontScale, GraphicsUnit.Point)) //probably don't need to redefine font here
                {
                    g.DrawString(text, font, Brushes.Black, rect, stringFormat);
                }
                g.Flush();
            }

            return bitmap;
        }


    }
}
