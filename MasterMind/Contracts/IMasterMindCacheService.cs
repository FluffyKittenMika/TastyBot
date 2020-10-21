using System;
using System.Collections.Generic;
using System.Text;
namespace MasterMind.Contracts
{
    public interface IMasterMindCacheService
    {
        MMUserCache RetrieveItem<MMUserCache>(string key);
        bool CacheExists(string key);
        void StoreItems<MMUserCache>(MMUserCache Items, string key);
        void RemoveCache(string key);
        void ResetGlobalCache();
    }
}
