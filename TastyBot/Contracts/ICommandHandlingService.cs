using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace TastyBot.Contracts
{
    public interface ICommandHandlingService
    {
        Task OnCommandExecuted(Optional<CommandInfo> command, ICommandContext context, IResult result);
        Task OnMessageReceivedAsync(SocketMessage socketMessage);
    }
}