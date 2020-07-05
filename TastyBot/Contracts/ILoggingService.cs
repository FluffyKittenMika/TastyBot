using Discord;
using System.Threading.Tasks;

namespace TastyBot.Contracts
{
    public interface ILoggingService
    {
        Task LogAsync(LogMessage msg);
        Task LogDebugMessage(string source, string message);
        void LogRainbowMessage(string source, string message);
    }
}