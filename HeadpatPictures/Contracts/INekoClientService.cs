using Enums.PictureServices;
using System.IO;
using System.Threading.Tasks;

namespace HeadpatPictures.Contracts
{
    public interface INekoClientService
    {
        Task<Stream> GetSFWNekoClientPictureAsync(RegularNekos Types, string Text = "");
    }
}