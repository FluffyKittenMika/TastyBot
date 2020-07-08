using Enums.PictureServices;
using System.IO;
using System.Threading.Tasks;

namespace HeadpatPictures.Contracts
{
    public interface ICatService
    {
        Task<Stream> ReturnCacheAction(CatItems catEnum);
    }
}