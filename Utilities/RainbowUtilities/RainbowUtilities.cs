using Discord;
using System;

namespace Utilities.RainbowUtilities
{
    public static class RainbowUtilities
    {
        private static readonly Random _random = new Random();

        public static Color CreateRainbowColor()
        {
            return new Color(_random.Next(0, 255), _random.Next(0, 255), _random.Next(0, 255));
        }

        public static ConsoleColor CreateConsoleRainbowColor()
        {
            int count = System.Enum.GetNames(typeof(ConsoleColor)).Length;
            return (ConsoleColor)typeof(ConsoleColor).GetEnumValues().GetValue(_random.Next(0, count));
        }
    }
}
