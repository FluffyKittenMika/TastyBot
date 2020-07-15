using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using DiscordUI.Contracts;
using Utilities.TasksUtilities;

namespace DiscordUI.Services
{
    public class PictureCacheService : IPictureCacheService
    {
        private readonly int MaximumCachedStreams;
        private string _cacheKey;
        private Type _pictureType;
        private dynamic _pictureTypeValue;
        private bool _linkSupport;
        private readonly getStreamFromAPI _getStream;

        public PictureCacheService(int MaximumCachedStreams, getStreamFromAPI getStreamFromAPI)
        {
            this.MaximumCachedStreams = MaximumCachedStreams;
            _getStream = getStreamFromAPI;
        }

        public delegate Task<Stream> getStreamFromAPI(string pictureTypeName, object[] optionalArgs = null);

        public async Task<Stream> ReturnFastestStream<T>(Func<string, List<Stream>> getStreamsFromCache,
            Action<List<Stream>, string> setCache,
            Func<string, bool> cacheExists,
            T pictureTypeValue,
            bool linkSupport = false)
        {
            InitVariables(pictureTypeValue, linkSupport);

            if (!cacheExists(_cacheKey))
            {
                FillCache(setCache).PerformAsyncTaskWithoutAwait();

                return await GetStreamFromAPITranslated();
            }
            else
            {
                List<Stream> cachedStreams = getStreamsFromCache(_cacheKey);
                ReplacePicture(setCache, cachedStreams).PerformAsyncTaskWithoutAwait();
                return cachedStreams.FirstOrDefault();
            }
        }

        private void InitVariables<T>(T pictureTypeValue, bool linkSupport)
        {
            _pictureType = typeof(T);
            _pictureTypeValue = pictureTypeValue;
            _cacheKey = $"{_pictureType.Name}.{_pictureTypeValue}";
            _linkSupport = linkSupport;
        }

        private async Task ReplacePicture(
            Action<List<Stream>, string> setCache,
            List<Stream> cachedStreams)
        {
            cachedStreams.Remove(cachedStreams.FirstOrDefault());
            cachedStreams.Add(await GetStreamFromAPITranslated());
            setCache(cachedStreams, _cacheKey);
        }

        private async Task FillCache(Action<List<Stream>, string> setCache)
        {
            List<Task<Stream>> streamTasks = new List<Task<Stream>>();

            for (int i = 0; i < MaximumCachedStreams; i++)
            {
                streamTasks.Add(GetStreamFromAPITranslated());
            }
            List<Stream> streams = (await Task.WhenAll(streamTasks)).ToList();

            setCache(streams, _cacheKey);
        }

        // TODO: Add link support
        private async Task<Stream> GetStreamFromAPITranslated()
        {
            if (_pictureType == typeof(string))
            {
                return await _getStream(_pictureTypeValue.ToString());
            }
            else
            {
                return await _getStream(_pictureType.Name, new object[] { _pictureTypeValue });
            }
        }
    }
}