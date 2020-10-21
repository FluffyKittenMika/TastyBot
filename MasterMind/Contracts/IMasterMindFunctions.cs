using Discord;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MasterMind.Contracts
{
    public interface IMasterMindFunctions
    {
        Bitmap AddGrayCircles(Graphics g, Bitmap bitPicture, int height, int width, string UserName, string NameColor = "white");
        Bitmap Emptyboard(int height, int width);
        System.Drawing.Color EmojiToColor(Emoji emoji);
        int ColorToNumber(System.Drawing.Color color);
        Bitmap SingleDotColorEditor(Graphics g, Bitmap bitPicture, int RowNumber, int ColumnNumber, System.Drawing.Color color, int width);
        List<int> NumberPattern(int NumberOfColumns);
        Bitmap AddACircleIndicator(int line, int column, System.Drawing.Color color, Bitmap bitmap);
        int WhiteNum(List<int> colorGuessNumber, List<int> SecretPattern, int widthX);
        int RedNum(List<int> colorGuess, List<int> SecretPattern);
        Bitmap RedAndWhiteDotEditor(Graphics g, Bitmap bitPicture, int width, int redAmount, int whiteAmount, int rowNum);
    }
}
