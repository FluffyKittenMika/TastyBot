using Discord;
using System;

using Interfaces.Contracts.Utilities;

namespace Utilities.RainbowUtilities
{
    public class RainbowUtilities : IRainbowUtilities
    {
        private readonly Random _random = new Random();

        public Color CreateRainbowColor()
        {
            return new Color(_random.Next(0, 255), _random.Next(0, 255), _random.Next(0, 255));
        }

        public ConsoleColor CreateConsoleRainbowColor()
        {
            int count = Enum.GetNames(typeof(ConsoleColor)).Length;
            return (ConsoleColor)typeof(ConsoleColor).GetEnumValues().GetValue(_random.Next(0, count));
        }
    }
}
