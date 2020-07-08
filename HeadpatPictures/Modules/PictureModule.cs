using Interfaces.Contracts.HeadpatPictures;

using HeadpatPictures.Contracts;
using HeadpatPictures.Utilities;

using System.Threading.Tasks;
using System.IO;

namespace HeadpatPictures.Modules
{
    public class PictureModule : IPictureModule
    {
        private readonly IPictureService _serv;

        public PictureModule(IPictureService serv)
        {
            _serv = serv;
        }

        public async Task<Stream> GetStreamFromEnumAsync<T>(T pictureTypeSubEnumValue, string text = "")
        {
            var stream = await _serv.ReturnCacheActionAsync(pictureTypeSubEnumValue);

            if (!PictureTypesUtilities.SubEnumIsGif(pictureTypeSubEnumValue) && text != "")
                stream = TextStreamWriter.WriteOnStream(stream, text);

            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }
    }
}
