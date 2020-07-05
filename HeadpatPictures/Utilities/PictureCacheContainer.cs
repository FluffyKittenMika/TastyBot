using System.Collections.Generic;
using System.IO;
using Cache.Contracts;
using HeadpatPictures.Contracts;

namespace HeadpatPictures.Utilities
{
    public class PictureCacheContainer : IPictureCacheContainer
    {
        private readonly ICacheContainer _cache;

        public PictureCacheContainer(ICacheContainer cache)
        {
            _cache = cache;
        }

        public bool Exists(string key)
        {
            return _cache.CacheExists(key);
        }

        public List<Stream> GetCachedPictures(string key)
        {
            return _cache.RetrieveItems<List<Stream>>(key);
        }

        public void SetCachedPictures(List<Stream> pictures, string key)
        {
            _cache.StoreItems(pictures, key);
        }

        public void ReplacePictures(List<Stream> pictures, string key)
        {
            _cache.RemoveCache(key);
            SetCachedPictures(pictures, key);
        }
    }
}
