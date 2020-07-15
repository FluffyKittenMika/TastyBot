using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;

using MasterMind.Services;
using MasterMind.Contracts;
using MasterMind.Entities;
using MasterMind.HelperClasses;
using MasterMind.Modules;

namespace MasterMind.Modules
{
    public class MasterMindModule : IMasterMindModule
    {
        private readonly IMasterMindService _service;
        public MasterMindModule(IMasterMindService service)
        {
            _service = service;
        }
        public bool GameIsRunningM(IUser user)
        {
            MasterMindUser masterMindUser = _service.GetUser(user);
            return masterMindUser.masterMindCommands.GameIsRunning();

        }
        public void StartGame(IUser user)
        {
            MasterMindUser masterMindUser = _service.GetUser(user);

        }

    }
}
