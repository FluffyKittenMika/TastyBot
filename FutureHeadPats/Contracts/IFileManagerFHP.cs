using System.Collections.Generic;
using System.Threading.Tasks;
using FutureHeadPats.Entities;
using FileManager.Contracts;
using FutureHeadPats.HelperClasses;

namespace FutureHeadPats.Contracts
{
    public interface IFileManagerFHP
    {
        List<FhpUser> LoadFhpUserData();
        void SaveFhpUserData(List<FhpUser> users);
    }
}