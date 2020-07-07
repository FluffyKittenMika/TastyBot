using HeadpatPictures.Contracts;

using Enums.PictureServices;

using System.Threading.Tasks;
using System.IO;

namespace HeadpatPictures.Modules
{
    public class NekoClientModule : INekoClientModule
    {
        private readonly INekoClientService _serv;
        public NekoClientModule(INekoClientService serv)
        {
            _serv = serv;
        }

        public async Task<Stream> ActionNekoClientPictureAsync(ActionNekos actionNekos)
        {
            var stream = await _serv.GetActionNekoClientPictureAsync(actionNekos);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        public async Task<Stream> SFWNekoClientPictureAsync(RegularNekos regularNekos, string text)
        {
            var stream = await _serv.GetSFWNekoClientPictureAsync(regularNekos, text);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        public async Task<Stream> NSFWNekoClientPictureAsync(NSFWNekos unregularNekos, string text)
        {
            var stream = await _serv.GetNSFWNekoClientPictureAsync(unregularNekos, text);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

    }
}
