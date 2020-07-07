using Discord;
using System;

namespace Interfaces.Contracts.Utilities
{
    public interface IRainbowUtilities
    {
        Color CreateRainbowColor();
        ConsoleColor CreateConsoleRainbowColor();
    }
}