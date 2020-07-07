using Discord.Commands;
using MasterMind.Contracts;
using Utilities.LoggingService;

namespace TastyBot.Modules
{
    /// <summary>
    /// The Module that handles all MasterMind Things
    /// </summary>
    [Name("MasterMind")]
    public class MasterMindModule : ModuleBase<SocketCommandContext>
    {
        private readonly IMasterMindModule _module;

        public MasterMindModule(IMasterMindModule module)
        {
            _module = module; 

            Logging.LogReadyMessage(this);
        }
    }
}
