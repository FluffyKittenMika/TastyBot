using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Utilities.TasksUtilities;

namespace Utilities.PictureUtilities
{
    public static class PictureCacheHandler
    {
        private static readonly int MaximumCachedStreams = 3;
        private static string _cacheKey;
        private static Type _pictureType;
        private static dynamic _pictureTypeValue;
        private static bool _linkSupport;

        public delegate Task<Stream> getStreamFromAPI(string pictureTypeName, object[] optionalArgs = null);

        public async static Task<Stream> ReturnFastestStream<T>(Func<string, List<Stream>> getStreamsFromCache,
            getStreamFromAPI getStreamFromAPI,
            Action<List<Stream>, string> setCache,
            Func<string, bool> cacheExists,
            T pictureTypeValue,
            bool linkSupport = false)
        {
            InitVariables(pictureTypeValue, linkSupport);

            if (!cacheExists(_cacheKey))
            {
                FillCache(setCache, getStreamFromAPI).PerformAsyncTaskWithoutAwait();

                return await GetStreamFromAPITranslated(getStreamFromAPI);
            }
            else
            {
                List<Stream> cachedStreams = getStreamsFromCache(_cacheKey);
                ReplacePicture(setCache, getStreamFromAPI, cachedStreams).PerformAsyncTaskWithoutAwait();
                return cachedStreams.FirstOrDefault();
            }
        }

        private static void InitVariables<T>(T pictureTypeValue, bool linkSupport)
        {
            _pictureType = typeof(T);
            _pictureTypeValue = pictureTypeValue;
            _cacheKey = $"{_pictureType.Name}.{_pictureTypeValue}";
            _linkSupport = linkSupport;
        }

        private async static Task ReplacePicture(
            Action<List<Stream>, string> setCache, 
            getStreamFromAPI getStreamFromAPI,
            List<Stream> cachedStreams)
        {
            cachedStreams.Remove(cachedStreams.FirstOrDefault());
            cachedStreams.Add(await GetStreamFromAPITranslated(getStreamFromAPI));
            setCache(cachedStreams, _cacheKey);
        }

        private async static Task FillCache(Action<List<Stream>, string> setCache,
            getStreamFromAPI getStreamFromAPI)
        {
            List<Task<Stream>> streamTasks = new List<Task<Stream>>();

            for (int i = 0; i < MaximumCachedStreams; i++)
            {
                streamTasks.Add(GetStreamFromAPITranslated(getStreamFromAPI));
            }
            List<Stream> streams = (await Task.WhenAll(streamTasks)).ToList();

            setCache(streams, _cacheKey);
        }

        // TODO: Add link support
        private async static Task<Stream> GetStreamFromAPITranslated(getStreamFromAPI getStreamFromAPI)
        {
            if (_pictureType == typeof(string))
            {
                return await getStreamFromAPI(_pictureTypeValue.ToString());
            }
            else
            {
                return await getStreamFromAPI(_pictureType.Name, new object[] { _pictureTypeValue });
            }
        }
    }
}