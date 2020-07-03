using Discord.Commands;
using MasterMind.Contracts;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace TastyBot.Modules
{
    /// <summary>
    /// The Module that handles all FHP related interactions
    /// </summary>
    [Name("MasterMind")]
    public class MasterMindModule : ModuleBase<SocketCommandContext>
    {
        private readonly IMasterMindModule _module;

        public MasterMindModule(IMasterMindModule module)
        {
            _module = module;
        }
    }
}
