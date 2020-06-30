using Discord;
using System.Threading.Tasks;

namespace TastyBot.Contracts
{
    public interface ILoggingService
    {
        Task OnLogAsync(LogMessage msg);
    }
}