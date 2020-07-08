using HeadpatPictures.Contracts;
using System.IO;

using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Utilities.LoggingService;
using HeadpatPictures.Utilities;
using Utilities.TasksManager;
using Enums.PictureServices;

namespace HeadpatPictures.Services
{
    public class CatService : ICatService
    {
        private readonly int MaxUniqueCounter = 6;

        public async Task<Stream> ReturnCacheAction(CatItems catEnum)
        {
            Stream stream;

            //Sets the name to Type + value
            string key = typeof(CatItems).Name + catEnum.ToString();

            //If amount of images is equal to 0, or the key does not exist, fill it
            if (CacheCommands.GetCacheCount(key) == 0 || !CacheCommands.Exists(key))
            {
                //Fills cache with pictures to MaxUniqueCounter
                FillPictureCache(key, catEnum).PerformAsyncTaskWithoutAwait();

                // Get an image directly from NekoClient while the cache is filling up
                stream = await CatPicture.GetCatItem(catEnum);
            }
            else
            {
                // Get the first image in the cache
                stream = CacheCommands.GetCachedPictures(key).FirstOrDefault();
                // Replace that image
                ReplaceCachePicture(key, catEnum).PerformAsyncTaskWithoutAwait();
            }
            return stream;
        }

        private async Task ReplaceCachePicture(string key, CatItems catEnum)
        {
            CacheCommands.RemoveFirstCachePicture(key);
            await AddNewCachePicture(key, catEnum);
        }

        private async Task AddNewCachePicture(string key, CatItems catEnum)
        {
            if (CacheCommands.GetCachedPictures(key).Count == MaxUniqueCounter)
            {
                await Logging.LogErrorMessage("NekoClientModule", $"Unable to add a picture to cache {key}, maximum pictures of {MaxUniqueCounter} has been reached.");
                return;
            }
            CacheCommands.AddCachePicture(await CatPicture.GetCatItem(catEnum), key);
        }

        private async Task FillPictureCache(string key, CatItems catEnum)
        {
            List<Task<Stream>> tasks = new List<Task<Stream>>();

            for (int i = 0; i < MaxUniqueCounter; i++)
            {
                tasks.Add(CatPicture.GetCatItem(catEnum));
            }

            List<Stream> picturesToCache = (await Task.WhenAll(tasks)).OfType<Stream>().ToList();
            CacheCommands.ReplacePictures(picturesToCache, key);
        }
    }
}
