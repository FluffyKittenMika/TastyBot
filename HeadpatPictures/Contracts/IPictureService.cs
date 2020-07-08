using System.IO;
using System.Threading.Tasks;

namespace HeadpatPictures.Contracts
{
    public interface IPictureService
    {
        Task<Stream> ReturnCacheActionAsync<T>(T nekoEnum);
    }
}