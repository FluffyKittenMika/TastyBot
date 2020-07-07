using HeadpatPictures.Contracts;

using System.Threading.Tasks;
using System.IO;
using System;
using Utilities.LoggingService;

namespace HeadpatPictures.Modules
{
    public class CatModule : ICatModule
    {
        private readonly ICatService _serv;

        public CatModule(ICatService serv)
        {
            _serv = serv;
        }

        public async Task<Stream> CatPictureAsync(int textSize, string color, string text)
        {
            var stream = await _serv.ReturnCacheAction("RegularBoringCat",text);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        public async Task<Stream> CatGifAsync()
        {
            var stream = await _serv.ReturnCacheAction("RegularAnimatedButBoringCat","");
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }
    }
}
