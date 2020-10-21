using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Xml.Schema;
using System.Linq;

using MasterMind.Contracts;

using Discord;
using Discord.WebSocket;

using Color = System.Drawing.Color;
using MasterMind.HelperClasses;

namespace MasterMind.Entities
{
    public class Line
    {
        public int LineNum { get; set; }
        public List<System.Drawing.Color> ColorGuessColor { get; set; }
        public List<int> ColorGuessNumber { get; set; }
        public int RedAmount { get; set; }

        public int WhiteAmount { get; set; }
    }


    public class MasterMindFunctions : IMasterMindFunctions
    {
        private Emoji blackEmoji = new Emoji("⚫");
        private Emoji yellowEmoji = new Emoji("🟡");
        private Emoji orangeEmoji = new Emoji("🟠");
        private Emoji purpleEmoji = new Emoji("🟣");
        private Emoji greenEmoji = new Emoji("🟢");
        private Emoji blueEmoji = new Emoji("🔵");
        private Emoji redEmoji = new Emoji("🔴");
        private Emoji ArrowEmoji = new Emoji("➡️");

        public Bitmap AddACircleIndicator(int line, int column, System.Drawing.Color color, Bitmap bitmap)
        {
            int numOfPizelHeight = (bitmap.Height - (line + 1) * 100) + 1;
            int numOfPixelWidth = (bitmap.Width - (column + 1) * 100) + 1;
            int Square = 49;
            Pen pen = new Pen(color);
            Graphics g = Graphics.FromImage(bitmap);
            for (int i = 0; i < 4; i++)
            {
                g.DrawEllipse(pen, numOfPixelWidth, numOfPizelHeight, Square, Square);
                Square -= 1;
                numOfPizelHeight += 1;
                numOfPixelWidth += 1;
            }
            return bitmap;
            
        }


        

        public int RedNum(List<int> colorGuess, List<int> SecretPattern)
        {
            int i = 0;
            int red = 0;

            foreach (int Num in SecretPattern)
            {
                if (Num == colorGuess[i])
                {
                    red++;
                }
                i++;
            }
            return red;
        }

        public int WhiteNum(List<int> colorGuessNumber, List<int> SecretPattern, int widthX) 
        {
            int x = 0;
            int y = 0;
            int white = 0;
            List<bool> RedNumber = new List<bool>();
            int l = 0;

            for (int i = 0; i < widthX; i++)
            {
                RedNumber.Add(false);
            }

            foreach (int numPattern in SecretPattern)
            {
                if (numPattern == colorGuessNumber[l])
                {
                    RedNumber[l] = true;
                }
                l++;
            }

            foreach (int num in SecretPattern)
            {
                foreach (int guess in colorGuessNumber)
                {
                    if (!RedNumber[x])
                    {
                        if (!RedNumber[y])
                        {
                            if (num == guess)
                            {
                                white++;
                            }
                        }
                    }
                    y++;
                }
                y = 0;
                x++;
            }
            return white;
        }

        public Bitmap Emptyboard(int height, int width)
        {
            //calculates the amount of pixels needed
            int numOfPizelHeight;
            int numOfPixelWidth;
            numOfPizelHeight = ((height * 2) + 1) * 50 + 50;
            numOfPixelWidth = width * 2;
            numOfPixelWidth = numOfPixelWidth * 50;
            numOfPixelWidth = numOfPixelWidth + ((width * 2 + 1) * 20);


            Bitmap bitPicture = new Bitmap(numOfPixelWidth, numOfPizelHeight);
            //changes every pixel color to gray
            for (var x = 0; x < bitPicture.Width; x++)
            {
                for (var y = 0; y < bitPicture.Height; y++)
                {
                    bitPicture.SetPixel(x, y, System.Drawing.Color.Transparent);
                }
            }
            //idk
            return bitPicture;
        }

        public Bitmap AddGrayCircles(Graphics g, Bitmap bitPicture, int height, int width, string UserName, string NameColor = "white")
        {

            Pen pPen = new Pen(System.Drawing.Color.Gray);
            float xcCircle = bitPicture.Width;
            float ycCircle = bitPicture.Height;
            float xsCircle = bitPicture.Width;
            float ysCircle = bitPicture.Height;
            float ydCircle = bitPicture.Height - 60;
            SolidBrush brush = new SolidBrush(System.Drawing.Color.Gray);
            SolidBrush brushh = new SolidBrush(System.Drawing.Color.Gray);
            //i actually dont know how his works but hey, it works :)
            for (int p = 0; p < height; p++)
            {
                ysCircle -= 100;
                ycCircle -= 100;

                for (int i = 0; i < width; i++)
                {
                    xcCircle -= 100;

                    g.DrawEllipse(pPen, xcCircle, ycCircle, 50, 50);
                    g.FillEllipse(brush, xcCircle, ycCircle, 50, 50);

                }
                xsCircle = xcCircle;
                for (int o = 0; o < width; o++)
                {
                    xsCircle -= 40;
                    g.DrawEllipse(pPen, xsCircle, ysCircle, 20, 20);
                    g.FillEllipse(brushh, xsCircle, ysCircle, 20, 20);
                    g.DrawEllipse(pPen, xsCircle, ydCircle, 20, 20);
                    g.FillEllipse(brushh, xsCircle, ydCircle, 20, 20);

                }

                xcCircle = bitPicture.Width;
                xsCircle = bitPicture.Width - width * 100;
                ydCircle -= 100;
            }
            //totally not copied from stackoverflow, plz dont judge :)
            /* MemoryStream streamPicture = new MemoryStream();
            bitPicture.Save(streamPicture, System.Drawing.Imaging.ImageFormat.Png);
            streamPicture.Seek(0, SeekOrigin.Begin);
            return streamPicture;
            */
            Rectangle rectangle = new Rectangle(0, 0, bitPicture.Width, 90);
            Color color = Color.FromName(NameColor);
            Font ffont = new Font("Arial", 32);
            StringFormat stringFormat = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center,
                Trimming = StringTrimming.EllipsisWord
            };

            SizeF s = g.MeasureString(UserName, ffont);
            float fontScale = Math.Max(s.Width / rectangle.Width, s.Height / rectangle.Height);
            using (Font font = new Font(ffont.FontFamily, ffont.SizeInPoints / fontScale, GraphicsUnit.Point)) //probably don't need to redefine font here
            {
                g.DrawString(UserName, font, new SolidBrush(color), rectangle, stringFormat);
            }
            g.Flush();

            return bitPicture;
        }
        
        public List<int> NumberPattern(int NumberOfColumns)
        {
            List<int> Pattern = new List<int>();
            Random rnd = new Random();
            for (int i = 0; i < NumberOfColumns; i++)
            {
                Pattern.Add(rnd.Next(0, 6));
            }
            return Pattern;
        }

        public int ColorToNumber(System.Drawing.Color color)
        {
            if (System.Drawing.Color.Black == color)
            {
                return 0;
            }
            else if (System.Drawing.Color.Yellow == color)
            {
                return 1;
            }
            else if (System.Drawing.Color.Orange == color)
            {
                return 2;
            }
            else if (System.Drawing.Color.Purple == color)
            {
                return 3;
            }
            else if (System.Drawing.Color.Green == color)
            {
                return 4;
            }
            else if (System.Drawing.Color.Blue == color)
            {
                return 5;
            }
            else if (System.Drawing.Color.Red == color)
            {
                return 6;
            }
            else if (System.Drawing.Color.Transparent == color)
            {
                return 7;
            }
            return 8;
        }

        
        /*
         Int        color
        0           Black
        1           Yellow
        2           Orange
        3           Purple
        4           Green
        5           Blue
        6           Red
        7           Empty (aka when the dot is gray)
        8           Arrow
         */
        public System.Drawing.Color EmojiToColor(Emoji emoji)
        {
            if (blackEmoji.Name == emoji.Name)
            {
                return System.Drawing.Color.Black;
            }
            else if (yellowEmoji.Name == emoji.Name)
            {
                return System.Drawing.Color.Yellow;
            }
            else if (orangeEmoji.Name == emoji.Name)
            {
                return System.Drawing.Color.Orange;
            }
            else if (purpleEmoji.Name == emoji.Name)
            {
                return System.Drawing.Color.Purple;
            }
            else if (greenEmoji.Name == emoji.Name)
            {
                return System.Drawing.Color.Green;
            }
            else if(blueEmoji.Name == emoji.Name)
            {
                return System.Drawing.Color.Blue;
            }
            else if(redEmoji.Name == emoji.Name)
            {
                return System.Drawing.Color.Red;
            }
            else if(ArrowEmoji.Name == emoji.Name)
            {
                return System.Drawing.Color.FromArgb(1, 1, 1, 1);
            }
            return System.Drawing.Color.Empty;

        }

        public Bitmap LineEditor(Bitmap bitPicture, int lineNum, List<System.Drawing.Color> colors, int amountRed, int amountWhite)
        {
            Graphics g = Graphics.FromImage(bitPicture);
            int pixelsHeight = bitPicture.Height;
            pixelsHeight -= lineNum * 100;
            int pixelWidth = bitPicture.Width;
            int pixelWidthh;
            foreach (System.Drawing.Color color in colors)
            {
                pixelWidth -= 100;
                Pen pen = new Pen(color);
                SolidBrush brush = new SolidBrush(color);
                g.DrawEllipse(pen, pixelsHeight, pixelWidth, 50, 50);
                g.FillEllipse(brush, pixelsHeight, pixelWidth, 50, 50);

            }
            pixelWidthh = pixelWidth;
            for (int i = 0; i < amountRed; i++)
            {
                pixelWidth -= 40;
                Pen pen = new Pen(System.Drawing.Color.Red);
                SolidBrush brush = new SolidBrush(System.Drawing.Color.Red);
                g.DrawEllipse(pen, pixelsHeight, pixelWidth, 50, 50);
                g.FillEllipse(brush, pixelsHeight, pixelWidth, 50, 50);
            }
            pixelWidth = pixelWidthh;
            pixelsHeight += 40;
            for (int i = 0; i < amountWhite; i++)
            {
                pixelWidth -= 40;
                Pen pen = new Pen(System.Drawing.Color.White);
                SolidBrush brush = new SolidBrush(System.Drawing.Color.White);
                g.DrawEllipse(pen, pixelsHeight, pixelWidth, 50, 50);
                g.FillEllipse(brush, pixelsHeight, pixelWidth, 50, 50);
            }
            return bitPicture;
        }

        public Bitmap SingleDotColorEditor(Graphics g, Bitmap bitPicture, int RowNumber, int ColumnNumber, System.Drawing.Color color, int width)
        {
            RowNumber += 1;
            int pixelsHeight = bitPicture.Height - RowNumber * 100;
            int pixelsWidth = bitPicture.Width - width * 100 + ColumnNumber * 100;
            Pen newPen = new Pen(color);
            SolidBrush newSolidBrush = new SolidBrush(color);
            g.DrawEllipse(newPen, pixelsWidth, pixelsHeight, 50, 50);
            g.FillEllipse(newSolidBrush, pixelsWidth, pixelsHeight, 50, 50);
            return bitPicture;
        }

        public Bitmap RedAndWhiteDotEditor(Graphics g, Bitmap bitPicture, int width, int redAmount, int whiteAmount, int rowNum)
        {
            int pixelInWidth = ((width * 2) + 1) * 20;
            int pixelInHeight = bitPicture.Height - (rowNum + 1) * 100;
            int pixelEdit = pixelInWidth;
            Pen Pw = new Pen(Color.White);
            Pen Pr = new Pen(Color.Red);
            SolidBrush Bw = new SolidBrush(Color.White);
            SolidBrush Br = new SolidBrush(Color.Red);

            for (int i = 0; i < whiteAmount; i++)
            {
                pixelEdit -= 40;
                g.DrawEllipse(Pw, pixelEdit, pixelInHeight, 20, 20);
                g.FillEllipse(Bw, pixelEdit, pixelInHeight, 20, 20);
            }
            pixelEdit = pixelInWidth;
            pixelInHeight += 40;
            for (int i = 0; i < redAmount; i++)
            {
                pixelEdit -= 40;
                g.DrawEllipse(Pr, pixelEdit, pixelInHeight, 20, 20);
                g.FillEllipse(Br, pixelEdit, pixelInHeight, 20, 20);
            }
            return bitPicture;
        }
    }
}
