using System.Threading.Tasks;
using TastyBot.Services;

namespace TastyBot
{
    class Program
    {
        public static Task Main(string[] args)
            => Startup.RunAsync(args);
    }
}