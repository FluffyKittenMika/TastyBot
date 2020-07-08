using Enums.PictureServices;
using HeadpatPictures.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Utilities.LoggingService;
using Utilities.TasksManager;

namespace HeadpatPictures.Utilities
{
    public class NekoClientService : INekoClientService
    {

        private readonly int MaxUniqueCounter = 3;

        public async Task<Stream> ReturnCacheAction<T>(T nekoEnum)
        {
            Type t = typeof(T);
            Stream stream;

            //Sets the name to Type + value
            string key = t.Name + nekoEnum.ToString();

            //If amount of images is equal to 0, or the key does not exist, fill it
            if (CacheCommands.GetCacheCount(key) == 0 || !CacheCommands.Exists(key))
            {
                //Fills cache with pictures to MaxUniqueCounter
                FillPictureCache(key, nekoEnum).PerformAsyncTaskWithoutAwait();

                // Get an image directly from NekoClient while the cache is filling up
                PictureTypes pictureType = PictureEnumUtilities.GetPictureTypeFromEnum(nekoEnum);
                stream = await NekoClient.GetNekoClientItem(pictureType, nekoEnum);
            }
            else
            {
                // Get the first image in the cache
                stream = CacheCommands.GetCachedPictures(key).FirstOrDefault();
                // Replace that image
                ReplaceCachePicture(key, nekoEnum).PerformAsyncTaskWithoutAwait();
            }
            return stream;
        }

        private async Task ReplaceCachePicture<T>(string key, T nekoEnum)
        {
            CacheCommands.RemoveFirstCachePicture(key);
            await AddNewCachePicture(key, nekoEnum);
        }

        private async Task AddNewCachePicture<T>(string key, T nekoEnum)
        {
            if (CacheCommands.GetCachedPictures(key).Count == MaxUniqueCounter)
            {
                await Logging.LogErrorMessage("NekoClientModule", $"Unable to add a picture to cache {key}, maximum pictures of {MaxUniqueCounter} has been reached.");
                return;
            }
            PictureTypes pictureType = PictureEnumUtilities.GetPictureTypeFromEnum(nekoEnum);
            CacheCommands.AddCachePicture(await NekoClient.GetNekoClientItem(pictureType, nekoEnum), key);
        }

        private async Task FillPictureCache<T>(string key, T nekoEnum)
        {
            PictureTypes pictureType = PictureEnumUtilities.GetPictureTypeFromEnum(nekoEnum);
            List<Task<Stream>> tasks = new List<Task<Stream>>();

            for (int i = 0; i < MaxUniqueCounter; i++)
            {
                tasks.Add(NekoClient.GetNekoClientItem(pictureType, nekoEnum));
            }

            List<Stream> picturesToCache = (await Task.WhenAll(tasks)).OfType<Stream>().ToList();
            CacheCommands.ReplacePictures(picturesToCache, key);
        }
    }
}
