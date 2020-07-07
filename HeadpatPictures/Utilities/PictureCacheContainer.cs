using System.Collections.Generic;
using System.IO;
using HeadpatPictures.Contracts;

using Utilities.Cache;

namespace HeadpatPictures.Utilities
{
    public class PictureCacheContainer : IPictureCacheContainer
    {

        public bool Exists(string key)
        {
            return Cache.CacheExists(key);
        }

        public int GetCacheCount(string key)
        {
            if (Exists(key))
                return GetCachedPictures(key).Count;
            return 0;
        }

        public List<Stream> GetCachedPictures(string key)
        {
            return Cache.RetrieveItems<List<Stream>>(key);
        }

        public void SetCachedPictures(List<Stream> pictures, string key)
        {
            Cache.StoreItems(pictures, key);
        }

        public void ReplacePictures(List<Stream> pictures, string key)
        {
            Cache.RemoveCache(key);
            SetCachedPictures(pictures, key);
        }
    }
}
