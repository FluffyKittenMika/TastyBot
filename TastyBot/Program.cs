using System.Threading.Tasks;
using DiscordUI.Services;

namespace DiscordUI
{
    class Program
    {
        public static Task Main(string[] args)
            => Startup.RunAsync(args);
    }
}