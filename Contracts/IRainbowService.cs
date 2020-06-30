using Discord;
using System.Collections.Generic;

namespace TastyBot.Contracts
{
    public interface IRainbowService
    {
        Color CreateRainbowColor();
        IEnumerable<Color> CreateRainbowColors(string textToRainbow);
    }
}