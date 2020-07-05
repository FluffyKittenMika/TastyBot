using Enums.PictureServices;

using System.IO;
using System.Threading.Tasks;

namespace HeadpatPictures.Contracts
{
    public interface INekoClientModule
    {
        Task<Stream> SFWNekoClientPictureAsync(RegularNekos regularNekos, string text);
    }
}