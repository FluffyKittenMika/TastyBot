using System;
using System.Collections.Generic;
using System.Text;

namespace TastyBot.Utility
{
    /// <summary>
    /// This is the bot config file, loaded from config.json
    /// </summary>
    public class Config
    {
        public string Prefix { get; set; }
        public int Savecounter { get; set; }
        public string DiscordToken { get; set; }
    }
}
