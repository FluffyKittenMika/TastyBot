using System.Collections.Generic;
using FutureHeadPats.Entities;

namespace FutureHeadPats.Contracts
{
    public interface IFileManagerFHP
    {
        List<FhpUser> LoadFhpUserData();
        void SaveFhpUserData(List<FhpUser> users);
    }
}