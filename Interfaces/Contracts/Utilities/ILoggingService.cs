using Discord;
using System.Threading.Tasks;

namespace Interfaces.Contracts.Utilities
{
    public interface ILoggingService
    {
        Task LogAsync(LogMessage msg);
        Task LogReadyMessage<T>(T Class);
        Task LogDebugMessage(string source, string message);
        void LogRainbowMessage(string source, string message);
    }
}