using Discord;
using System;
using System.Collections.Generic;
using System.Text;
using MasterMind.HelperClasses;

namespace MasterMind.Contracts
{
    public interface IMasterMindDataBase
    {
        MasterMindDBUser GetMMDBUser(IUser user);
        bool Deleteuser(IUser user);
    }
}
