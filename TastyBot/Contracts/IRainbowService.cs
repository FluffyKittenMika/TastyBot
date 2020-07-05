using Discord;
using System;

namespace TastyBot.Contracts
{
    public interface IRainbowService
    {
        Color CreateRainbowColor();
        ConsoleColor CreateConsoleRainbowColor();
    }
}