using MusicPlayer.Contracts;
using MusicPlayer.HelperClasses;
using System;
using System.Collections.Generic;
using System.Text;
using Utilities.PictureUtilities;

namespace DiscordUI.Services
{
    public class MusicPlayerCacheService : IMusicPlayerCacheService
    {
        public MusicDiscCache RetrieveItem<MusicDiscCache>(string key)
        {
            key = $"MDC{key}";
            return Cache.RetrieveItems<MusicDiscCache>(key);
        }

        public bool CacheExists(string key)
        {
            key = $"MDC{key}";
            return Cache.CacheExists(key);
        }

        public void StoreItems<MusicDiscCache>(MusicDiscCache Items, string key)
        {
            key = $"MDC{key}";
            Cache.StoreItems(Items, key);
        }

        public void RemoveCache(string key)
        {
            key = $"MDC{key}";
            Cache.RemoveCache(key);
        }

        public void ResetGlobalCache()
        {
            Cache.ResetGlobalCache();
        }
    }
}
