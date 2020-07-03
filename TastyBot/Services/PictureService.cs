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

        public async Task<Stream> GetNekoPictureAsync(string Text = " ", int Size = 32)
        {
            //Request the neko url
            Request Req = await NekoClient.Image_v3.Neko();

            //process it into a bitmap
            var resp = await _http.GetAsync(Req.ImageUrl);

            Stream s = await resp.Content.ReadAsStreamAsync();

            s = WriteOnStream(s, Text, Size);

            //bitmap = WriteOnBitmap(bitmap, Text, Size);
            Console.WriteLine(Req.ImageUrl);
            return s;
        }

        //TODO: make async, i haven't the clue go'vna on how, not that it works yet :)
        public Stream WriteOnStream(Stream stream, string text = " ", int Size = 32)
        {
            Bitmap bitmap = new Bitmap(stream);

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
            MemoryStream resultstream = new MemoryStream();
            bitmap.Save(resultstream, ImageFormat.Png);

            return resultstream;
        }


        /// <summary>
        /// Check if stream is png by use of magic numbers :)
        /// </summary>
        /// <param name="array"></param>
        /// <returns>True if it's an PNG</returns>
        public bool IsPng(byte[] array)
        {
            return array != null
                && array.Length > 8
                && array[0] == 0x89
                && array[1] == 0x50
                && array[2] == 0x4e
                && array[3] == 0x47
                && array[4] == 0x0d
                && array[5] == 0x0a
                && array[6] == 0x1a
                && array[7] == 0x0a;
        }
    }
}
