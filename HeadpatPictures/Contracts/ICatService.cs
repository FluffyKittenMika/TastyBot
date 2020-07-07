using System.IO;
using System.Threading.Tasks;

namespace HeadpatPictures.Contracts
{
    public interface ICatService
    {
        Task<Stream> GetCatGifAsync();
        Task<Stream> ReturnCacheAction(string key, string text);
    }
}