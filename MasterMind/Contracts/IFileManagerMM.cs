using MasterMind.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MasterMind.Contracts
{
    public interface IFileManagerMM
    {
        List<MasterMindUser> LoadMasterMindUserData();
        void SaveMasterMindUserData(List<MasterMindUser> users);
    }
}
