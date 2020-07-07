using System.Runtime.Caching;
using Interfaces.Contracts.Utilities;

namespace Utilities.Cache
{
    public class CacheContainer : ICacheContainer
    {
        private readonly MemoryCache _globalCache;

        public CacheContainer()
        {
            _globalCache = new MemoryCache("GlobalCache");
        }

        public T RetrieveItems<T>(string key)
        {
            T cacheObjects = (T)_globalCache.Get(key);
            return cacheObjects;
        }

        public bool CacheExists(string key)
        {
            bool cacheExists = _globalCache.Contains(key);
            return cacheExists;
        }

        public void StoreItems<T>(T items, string key)
        {
            var cacheItemPolicy = new CacheItemPolicy();

            _globalCache.Add(key, items, cacheItemPolicy);
        }

        public void RemoveCache(string key)
        {
            _globalCache.Remove(key);
        }

        public void ResetGlobalCache()
        {
            _globalCache.Dispose();
        }
    }
}
