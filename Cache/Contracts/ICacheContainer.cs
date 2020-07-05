namespace Cache.Contracts
{
    public interface ICacheContainer
    {
        void RemoveCache(string key);
        void ResetGlobalCache();
        bool CacheExists(string key);
        T RetrieveItems<T>(string key);
        void StoreItems<T>(T items, string key);
    }
}