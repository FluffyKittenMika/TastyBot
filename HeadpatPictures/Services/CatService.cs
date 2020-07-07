using HeadpatPictures.Contracts;
using System.IO;
using System.Net.Http;

using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;
using Utilities.LoggingService;

namespace HeadpatPictures.Services
{
    public class CatService : ICatService
    {
        private readonly HttpClient _http;
        private readonly IPictureCacheContainer _pictureCache;
        private readonly ITextStreamWriter _writer;

        private readonly int MaxUniqueCounter = 3;

        public CatService(IPictureCacheContainer pictureCache, ITextStreamWriter writer)
        {
            _http = new HttpClient();
            _pictureCache = pictureCache;
            _writer = writer;
        }

        public async Task<Stream> GetCatGifAsync()
        {
            var resp = await _http.GetAsync("https://cataas.com/cat/gif");
            return await resp.Content.ReadAsStreamAsync();
        }

        private async Task<Stream> GetCatPictureAsync()
        {
            var resp = await _http.GetAsync($"https://cataas.com/cat");
            return await resp.Content.ReadAsStreamAsync();
        }


        public async Task<Stream> ReturnCacheAction(string key, string text = "")
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            //If amount of images is equal to 0, or the key does not exist, fill it
            if (_pictureCache.GetCacheCount(key) == 0 || !_pictureCache.Exists(key))
                await FillPictureCache(key); //Fills it to MaxUniqueCounter

            //Get the first image from the cache
            Stream stream = _pictureCache.GetCachedPictures(key).FirstOrDefault();

            //Remove it from the cache, and replace it with a new.
            await ReplaceCachePicture(key);

            watch.Stop();
            await Logging.LogDebugMessage("Cache", $"Fetching image took: {watch.ElapsedMilliseconds}ms - Key:{key}");

            return _writer.WriteOnStream(stream, text);
        }


        private async Task ReplaceCachePicture(string key)
        {
            List<Stream> pictures = _pictureCache.GetCachedPictures(key);
            pictures.Remove(pictures.FirstOrDefault());
            pictures.Add(await GetCatPictureAsync());
            _pictureCache.ReplacePictures(pictures, key);
        }

        private async Task FillPictureCache(string key)
        {
            List<Stream> picturesToCache = new List<Stream>();

            for (int i = 0; i < MaxUniqueCounter; i++)
                picturesToCache.Add(await GetCatPictureAsync());

            _pictureCache.ReplacePictures(picturesToCache, key);
        }

    }
}
