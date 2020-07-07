using Enums.PictureServices;

using System.IO;
using System.Threading.Tasks;

namespace HeadpatPictures.Contracts
{
    public interface INekoClientModule
    {

        Task<Stream> ActionNekoClientGifAsync(ActionNekos actionNekos);
        Task<Stream> NSFWNekoClientGifAsync(AnimatedNSFWNekos unregularNekos);
        Task<Stream> SFWNekoClientGifAsync(AnimatedNekos regularNekos);



        Task<Stream> SFWNekoClientPictureAsync(RegularNekos regularNekos, string text);
        Task<Stream> NSFWNekoClientPictureAsync(NSFWNekos unregularNekos, string text);

    }
}