using Discord;

using System;
using System.IO;
using System.Threading.Tasks;
using System.Text;
using System.Threading;

namespace Utilities.LoggingService
{
    using Utilities.RainbowUtilities;

    public static class Logging
    {
        private static readonly string _logDirectory = Path.Combine(AppContext.BaseDirectory, "logs");
        private static readonly string _logFile = Path.Combine(_logDirectory, $"{DateTime.UtcNow:yyyy-MM-dd}.txt");
        private static readonly int _writeAttempts = 10;

        public static Task LogAsync(LogMessage msg)
        {
            CreateFilesIfNotExist();

            string logText = $"{DateTime.UtcNow:hh:mm:ss} [{msg.Severity}] {msg.Source}: {msg.Exception?.ToString() ?? msg.Message} \n";

            FileStream fileStream = WaitForFile(_logFile, FileMode.Append, FileAccess.Write, FileShare.Write);
            WriteToFile(fileStream, logText);
            fileStream.Close();


            Console.ForegroundColor = msg.Severity switch
            {
                LogSeverity.Critical => ConsoleColor.DarkRed,
                LogSeverity.Error => ConsoleColor.Red,
                LogSeverity.Warning => ConsoleColor.Yellow,
                LogSeverity.Info => ConsoleColor.Cyan,
                LogSeverity.Verbose => ConsoleColor.Blue,
                LogSeverity.Debug => ConsoleColor.Green,
                _ => ConsoleColor.Magenta,
            };
            return Console.Out.WriteAsync(logText);       // Write the log text to the console
        }

        public static void LogReadyMessage<T>(T Class)
        {
            string source = Class.GetType().Name;
            LogMessage logMessage = new LogMessage(LogSeverity.Info, source, "Ready");
            LogAsync(logMessage);
        }

        public static Task LogDebugMessage(string source, string message)
        {
            LogMessage logMessage = new LogMessage(LogSeverity.Debug, source, message);
            return LogAsync(logMessage);
        }

        public static Task LogErrorMessage(string source, string message)
        {
            LogMessage logMessage = new LogMessage(LogSeverity.Error, source, message);
            return LogAsync(logMessage);
        }

        public static Task LogCriticalMessage(string source, string message)
        {
            LogMessage logMessage = new LogMessage(LogSeverity.Critical, source, message);
            return LogAsync(logMessage);
        }

        public static Task LogInfoMessage(string source, string message)
        {
            LogMessage logMessage = new LogMessage(LogSeverity.Info, source, message);
            return LogAsync(logMessage);
        }

        public static Task LogWarningMessage(string source, string message)
        {
            LogMessage logMessage = new LogMessage(LogSeverity.Warning, source, message);
            return LogAsync(logMessage);
        }

        public static Task LogRainbowMessage(string source, string message)
        {
            CreateFilesIfNotExist();

            string logText = $"{DateTime.UtcNow:hh:mm:ss} [Rainbow] {source}: {message}";

            FileStream fileStream = WaitForFile(_logFile, FileMode.Append, FileAccess.Write, FileShare.Write);
            WriteToFile(fileStream, logText);
            fileStream.Close();

            foreach (char c in logText)
            {
                Console.ForegroundColor = RainbowUtilities.CreateConsoleRainbowColor();
                Console.Write(c);
            }
            return Console.Out.WriteLineAsync("");
        }

        private static void WriteToFile(FileStream fs, string log)
        {
            if (fs == null)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Out.WriteLineAsync($"{DateTime.UtcNow:yyyy-MM-dd} [Critical] Could not write to logs after {_writeAttempts} attemps.");
                return;
            }

            byte[] info = new UTF8Encoding(true).GetBytes(log);
            fs.Write(info, 0, info.Length);
        }

        private static void CreateFilesIfNotExist()
        {
            if (!Directory.Exists(_logDirectory))     // Create the log directory if it doesn't exist
                Directory.CreateDirectory(_logDirectory);
            if (!File.Exists(_logFile))               // Create today's log file if it doesn't exist
                File.Create(_logFile).Dispose();
        }

        private static FileStream WaitForFile(string fullPath, FileMode mode, FileAccess access, FileShare share)
        {
            for (int numTries = 0; numTries < _writeAttempts; numTries++)
            {
                FileStream fs = null;
                try
                {
                    fs = new FileStream(fullPath, mode, access, share);
                    return fs;
                }
                catch (IOException)
                {
                    if (fs != null)
                    {
                        fs.Dispose();
                    }
                    Thread.Sleep(50);
                }
            }

            return null;
        }
    }
}