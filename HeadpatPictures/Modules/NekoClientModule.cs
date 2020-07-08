using HeadpatPictures.Contracts;

using System.Threading.Tasks;
using System.IO;
using HeadpatPictures.Utilities;

namespace HeadpatPictures.Modules
{
    public class NekoClientModule : INekoClientModule
    {
        private readonly INekoClientService _serv;

        public NekoClientModule(INekoClientService neko)
        {
            _serv = neko;
        }

        public async Task<Stream> NekoClientItemAsync<T>(T nekoClientEnum, string text = "")
        {
            var stream = await _serv.ReturnCacheAction(nekoClientEnum);

            if (!PictureEnumUtilities.IsGif(nekoClientEnum) && text != "")
                stream = TextStreamWriter.WriteOnStream(stream, text);

            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }
    }
}
