using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace HeadpatPictures.Utilities
{
    public static class TextStreamWriter
    {
        //TODO: Text wrapping to fit more text into the fucking thing
        //TODO: Enable rainbow text
        /// <summary>
        /// Writes text on a given imagestream.
        /// </summary>
        /// <param name="stream">Image Stream</param>
        /// <param name="text">Text for the image</param>
        /// <returns>PNG formatted memorystream</returns>
        public static Stream WriteOnStream(Stream stream, string text)
        {
            if (text == "") return stream;
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

        /// <summary>
        /// Gets the color of a string.
        /// Only uses the first word.
        /// </summary>          
        /// <param name="check">Input string to check, if there's a color it also shortens the string to not include it</param>
        /// <returns>Correct color, default Black</returns>
        private static Color GetColor(ref string check)
        {
            string Col = check.Split(' ').FirstOrDefault();
            Color color = Color.FromName(Col);
            if (!color.IsKnownColor) //checks if known
                color = Color.FromName("black"); //if not, black it is!
            else
                check = check.Substring(check.IndexOf(' ') + 1); // skips first word
            return color;
        }
    }
}
