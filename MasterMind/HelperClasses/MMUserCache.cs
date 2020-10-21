using Discord;
using Discord.Rest;
using Discord.WebSocket;
using MasterMind.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MasterMind.HelperClasses
{
    public class MMUserCache
    {
        public bool CurrentGameRunning { get; set; } 
        public int DotsInWidth { get; set; }
        public int DotsInHeight { get; set; }
        public List<Line> Lines { get; set; }
        public Bitmap bitPicture { get; set; }
        public int DotColumnPostion { get; set; }
        public int CurrentLine { get; set; }
        public System.Drawing.Color DotIndicator { get; set; }
        public List<RestUserMessage> Messages { get; set; }
        public List<int> SecretPattern { get; set; }
        public Graphics ImageGraphics { get; set; }
    }
}
