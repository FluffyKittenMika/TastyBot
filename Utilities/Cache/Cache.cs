using System.Runtime.Caching;

namespace Utilities.PictureUtilities
{
    public static class Cache
    {
        private static readonly MemoryCache _globalCache = new MemoryCache("GlobalCache");

        public static T RetrieveItems<T>(string key)
        {
            T cacheObjects = (T)_globalCache.Get(key);
            return cacheObjects;
        }

        public static bool CacheExists(string key)
        {
            bool cacheExists = _globalCache.Contains(key);
            return cacheExists;
        }

        public static void StoreItems<T>(T items, string key)
        {
            var cacheItemPolicy = new CacheItemPolicy();

            _globalCache.Add(key, items, cacheItemPolicy);
        }

        public static void RemoveCache(string key)
        {
            _globalCache.Remove(key);
        }

        public static void ResetGlobalCache()
        {
            _globalCache.Dispose();
        }
    }
}
