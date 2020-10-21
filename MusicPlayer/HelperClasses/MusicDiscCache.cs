using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection.Metadata;
using System.Text;

namespace MusicPlayer.HelperClasses
{
    public class MusicDiscCache
    {
        public string SongName { get; set; }
        public int QueuePos { get; set; }
        public int SongDuration { get; set; }
        public Bitmap SongTumbNail { get; set; }
        public int RequestedByUserId { get; set; }
        public string MusicDirectory { get; set; } 
        //needs a mp3 or something file added bc how would you play the freaking song
    }
}
