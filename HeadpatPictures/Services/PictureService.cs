using HeadpatPictures.Contracts;

using Utilities.LoggingService;
using Utilities.TasksManager;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HeadpatPictures.Utilities
{
    public class PictureService : IPictureService
    {
        private readonly IPictureHub _hub;

        private readonly int MaxUniqueCounter = 3;

        public PictureService(IPictureHub hub)
        {
            _hub = hub;
        }

        public async Task<Stream> ReturnCacheActionAsync<T>(T pictureTypeSubEnumValue)
        {
            Type pictureTypeSubEnum = typeof(T);
            Stream stream;

            //Sets the name to Type + value
            string key = pictureTypeSubEnum.Name + pictureTypeSubEnumValue.ToString();

            //If the cache from the key does not exist, fill it
            if (!CacheCommands.Exists(key))
            {
                //Fills cache with pictures to MaxUniqueCounter
                FillPictureCacheAsync(key, pictureTypeSubEnumValue).PerformAsyncTaskWithoutAwait();

                // Get an image directly from the PictureHub while the cache is filling up
                stream = await _hub.GetPictureAsync(pictureTypeSubEnumValue);
            }
            else
            {
                // Get the first image in the cache
                stream = CacheCommands.GetCachedPictures(key).FirstOrDefault();
                // Replace that image parallel
                ReplaceCachePictureAsync(key, pictureTypeSubEnumValue).PerformAsyncTaskWithoutAwait();
            }
            return stream;
        }

        private async Task ReplaceCachePictureAsync<T>(string key, T pictureTypeSubEnumValue)
        {
            CacheCommands.RemoveFirstCachePicture(key);
            await AddNewCachePictureAsync(key, pictureTypeSubEnumValue);
        }

        private async Task AddNewCachePictureAsync<T>(string key, T pictureTypeSubEnumValue)
        {
            if (CacheCommands.GetCachedPictures(key).Count == MaxUniqueCounter)
            {
                Logging.LogErrorMessage(typeof(PictureService).Name, $"Unable to add a picture to cache {key}, maximum pictures of {MaxUniqueCounter} has been reached.").PerformAsyncTaskWithoutAwait();
                return;
            }
            CacheCommands.AddCachePicture(await _hub.GetPictureAsync(pictureTypeSubEnumValue), key);
        }

        private async Task FillPictureCacheAsync<T>(string key, T pictureTypeSubEnumValue)
        {
            List<Task<Stream>> tasks = new List<Task<Stream>>();

            for (int i = 0; i < MaxUniqueCounter; i++)
            {
                tasks.Add(_hub.GetPictureAsync(pictureTypeSubEnumValue));
            }

            List<Stream> picturesToCache = (await Task.WhenAll(tasks)).OfType<Stream>().ToList();
            CacheCommands.ReplacePictures(picturesToCache, key);
        }
    }
}
