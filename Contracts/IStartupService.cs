using System.Threading.Tasks;

namespace TastyBot.Contracts
{
    public interface IStartupService
    {
        Task StartAsync();
    }
}