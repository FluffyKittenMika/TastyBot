using FutureHeadPats.Entities;

using Discord;
using System.Collections.Generic;

namespace FutureHeadPats.Contracts
{
    public interface IHeadpatService
    {
        bool DeleteUser(IUser user);
        List<FhpUser> GetLeaderboard();
        FhpUser GetUser(IUser user);
        void Save();
    }
}