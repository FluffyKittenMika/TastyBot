using DiscordUI.Contracts;
using MasterMind.HelperClasses;
using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Text;
using Utilities.PictureUtilities;
using MasterMind.Contracts;

namespace DiscordUI.Services
{
    public class MasterMindCacheService : IMasterMindCacheService
    {
        public MMUserCache RetrieveItem<MMUserCache>(string key)
        {
            key = $"MM{key}";
            return Cache.RetrieveItems<MMUserCache>(key);
        }

        public bool CacheExists(string key)
        {
            key = $"MM{key}";
            return Cache.CacheExists(key);
        }

        public void StoreItems<MMUserCache>(MMUserCache Items, string key)
        {
            key = $"MM{key}";
            Cache.StoreItems(Items, key);
        }

        public void RemoveCache(string key)
        {
            key = $"MM{key}";
            Cache.RemoveCache(key);
        }

        public void ResetGlobalCache()
        {
            Cache.ResetGlobalCache();
        }
    }
}
