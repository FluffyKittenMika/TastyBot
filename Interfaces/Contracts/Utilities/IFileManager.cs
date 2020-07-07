using System.Collections.Generic;
using System.Threading.Tasks;

namespace Interfaces.Contracts.Utilities
{
    public interface IFileManager
    {
        void Init();
        Task<List<T>> LoadData<T>(string id);
        Task SaveData<T>(List<T> data, string id);
    }
}