using System.IO;
using System.Threading.Tasks;

namespace HeadpatPictures.Contracts
{
    public interface INekoClientService
    {
        Task<Stream> ReturnCacheAction<T>(T nekoEnum);
    }
}