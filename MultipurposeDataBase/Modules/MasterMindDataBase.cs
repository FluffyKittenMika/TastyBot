using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using MasterMind.Contracts;
using MasterMind.HelperClasses;
using MultipurposeDataBase.Contracts;
using MultipurposeDataBase.HelperClasses;

namespace MultipurposeDataBase.Modules
{
    public class MasterMindDataBase : IMasterMindDataBase
    {
        private readonly IDBService _DbService;
        public MasterMindDataBase(IDBService dBService)
        {
            _DbService = dBService;
        }
        public MasterMindDBUser GetMMDBUser(IUser user)
        {
            MDBUser MDBUser = _DbService.GetUser(user);
            return MDBUser.MMGame;
        }
        public bool Deleteuser(IUser user)
        {
            return _DbService.DeleteUser(user);
        }
    }
}
