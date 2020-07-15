using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;

using MasterMind.Modules;


namespace MasterMind.Contracts
{
    public interface IMasterMindModule
    {
        bool GameIsRunningM(IUser user);
    }
}
