using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileManager.Contracts
{
    public interface IFileManager
    {
        void Init();
        Task<List<T>> LoadData<T>(string id);
        Task SaveData<T>(List<T> data, string id);
    }
}