using System;
using System.Collections.Generic;
using System.Text;

namespace MusicPlayer.Contracts
{
    public interface IMusicPlayerCacheService
    {
        MusicDiscCache RetrieveItem<MusicDiscCache>(string key);
        bool CacheExists(string key);
        void StoreItems<MusicDiscCache>(MusicDiscCache Items, string key);
        void RemoveCache(string key);
        void ResetGlobalCache();
    }
}
