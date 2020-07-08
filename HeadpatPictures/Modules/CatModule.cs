using HeadpatPictures.Contracts;

using System.Threading.Tasks;
using System.IO;
using Enums.PictureServices;
using HeadpatPictures.Utilities;

namespace HeadpatPictures.Modules
{
    public class CatModule : ICatModule
    {
        private readonly ICatService _serv;

        public CatModule(ICatService serv)
        {
            _serv = serv;
        }

        public async Task<Stream> GetCatItemAsync(CatItems catEnum, string text)
        {
            var stream = await _serv.ReturnCacheAction(catEnum);

            if (!CatItemsEnumUtilities.IsGif(catEnum) && text != "")
                stream = TextStreamWriter.WriteOnStream(stream, text);

            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }
    }
}
