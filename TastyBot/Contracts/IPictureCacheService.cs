using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DiscordUI.Services;

namespace DiscordUI.Contracts
{
    public interface IPictureCacheService
    {
        Task<Stream> ReturnFastestStream<T>(Func<string, List<Stream>> getStreamsFromCache, 
            Action<List<Stream>, string> setCache, 
            Func<string, bool> cacheExists, 
            T pictureTypeValue, 
            bool linkSupport = false);
    }
}