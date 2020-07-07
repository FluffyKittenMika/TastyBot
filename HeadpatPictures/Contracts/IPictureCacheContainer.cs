using System.Collections.Generic;
using System.IO;

namespace HeadpatPictures.Contracts
{
    public interface IPictureCacheContainer
    {
        bool Exists(string key);
        List<Stream> GetCachedPictures(string key);
        void ReplacePictures(List<Stream> pictures, string key);
        void SetCachedPictures(List<Stream> pictures, string key);

        int GetCacheCount(string key);
    }
}