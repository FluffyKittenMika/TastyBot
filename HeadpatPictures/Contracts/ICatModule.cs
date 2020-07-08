using Enums.PictureServices;
using System.IO;
using System.Threading.Tasks;

namespace HeadpatPictures.Contracts
{
    public interface ICatModule
    {
        Task<Stream> GetCatItemAsync(CatItems catEnum, string text);
    }
}