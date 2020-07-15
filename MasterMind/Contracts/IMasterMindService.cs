using System;
using System.Collections.Generic;
using System.Text;

using Discord;

using MasterMind.Entities;
using MasterMind.Services;

namespace MasterMind.Contracts
{
    public interface IMasterMindService
    {
        bool DeleteUser(IUser user);
        List<MasterMindUser> GetLeaderboard();
        MasterMindUser GetUser(IUser user);
        void Save();
    }
}
