using Discord;
using MultipurposeDataBase.HelperClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultipurposeDataBase.Contracts
{
    public interface IDBService
    {
        bool DeleteUser(IUser user);
        MDBUser GetUser(IUser user);
        void Save();
    }
}
