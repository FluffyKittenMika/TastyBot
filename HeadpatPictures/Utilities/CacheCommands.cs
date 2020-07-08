using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utilities.Cache;

namespace HeadpatPictures.Utilities
{
    public static class CacheCommands
    {
        public static bool Exists(string key)
        {
            return Cache.CacheExists(key);
        }

        public static int GetCacheCount(string key)
        {
            if (Exists(key))
                return GetCachedPictures(key).Count;
            return 0;
        }

        public static List<Stream> GetCachedPictures(string key)
        {
            return Cache.RetrieveItems<List<Stream>>(key);
        }

        public static void SetCachedPictures(List<Stream> pictures, string key)
        {
            Cache.StoreItems(pictures, key);
        }

        public static void ReplacePictures(List<Stream> pictures, string key)
        {
            Cache.RemoveCache(key);
            SetCachedPictures(pictures, key);
        }

        public static void RemoveFirstCachePicture(string key)
        {
            List<Stream> pictures = GetCachedPictures(key);
            pictures.Remove(pictures.FirstOrDefault());
            ReplacePictures(pictures, key);
        }

        public static void AddCachePicture(Stream picture, string key)
        {
            List<Stream> pictures = GetCachedPictures(key);
            pictures.Add(picture);
            ReplacePictures(pictures, key);
        }
    }
}
