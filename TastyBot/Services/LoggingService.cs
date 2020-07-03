using TastyBot.Contracts;

using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.IO;
using System.Threading.Tasks;

namespace TastyBot.Services
{
    public class LoggingService : ILoggingService
    {
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;

        private readonly string _logDirectory;
        private string _logFile => Path.Combine(_logDirectory, $"{DateTime.UtcNow:yyyy-MM-dd}.txt");

        // DiscordSocketClient and CommandService are injected automatically from the IServiceProvider
        public LoggingService(DiscordSocketClient discord, CommandService commands)
        {
            _logDirectory = Path.Combine(AppContext.BaseDirectory, "logs");

            _discord = discord;
            _commands = commands;

            _discord.Log += OnLogAsync;
            _commands.Log += OnLogAsync;
        }

        public Task OnLogAsync(LogMessage msg)
        {
            if (!Directory.Exists(_logDirectory))     // Create the log directory if it doesn't exist
                Directory.CreateDirectory(_logDirectory);
            if (!File.Exists(_logFile))               // Create today's log file if it doesn't exist
                File.Create(_logFile).Dispose();

            string logText = $"{DateTime.UtcNow:hh:mm:ss} [{msg.Severity}] {msg.Source}: {msg.Exception?.ToString() ?? msg.Message}";
            try
            {
                File.AppendAllText(_logFile, logText + "\n");     // Write the log text to a file
            }
            catch (Exception e)
            {
                //Fuck crashing the bot if it's dual writing
                Console.WriteLine("Bot tried to write to open file when logging, " + e.Message);
            }

            switch (msg.Severity)
            {
                case LogSeverity.Critical:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogSeverity.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogSeverity.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogSeverity.Info:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case LogSeverity.Verbose:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case LogSeverity.Debug:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
            }

            return Console.Out.WriteLineAsync(logText);       // Write the log text to the console
        }
    }
}