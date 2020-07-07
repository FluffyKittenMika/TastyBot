using HeadpatPictures.Contracts;
using System.IO;
using System.Net.Http;

using System.Threading.Tasks;
using System.Collections.Generic;

namespace HeadpatPictures.Services
{
    public class CatService : ICatService
    {
        private readonly HttpClient _http;
        private readonly IPictureCacheContainer _pictureCache;
        private readonly ITextStreamWriter _writer;

        private int CachedPictureCount = 0;
        private readonly int MaxCachedItemCount = 3;
        private readonly string CatPicturesCache = "CatPictures";
        private readonly string CatGifsCache = "CatGifs";

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

        public async Task<Stream> ReturnCachedOrNormalPicture(string text)
        {
            if (CachedPictureCount > MaxCachedItemCount || !_pictureCache.Exists(CatPicturesCache))
            {
                CachedPictureCount = 0;
                await FillPictureCache();
                Stream catPicture = await GetCatPictureAsync();
                return _writer.WriteOnStream(catPicture, text);
            }
            else
            {
                await ReplaceCachePicture();
                Stream stream = _pictureCache.GetCachedPictures(CatPicturesCache)[CachedPictureCount];
                return _writer.WriteOnStream(stream, text);
            }
        }

        private async Task ReplaceCachePicture()
        {
            List<Stream> pictures = _pictureCache.GetCachedPictures(CatPicturesCache);
            pictures.Remove(pictures[CachedPictureCount]);
            pictures.Add(await GetCatPictureAsync());
            _pictureCache.ReplacePictures(pictures, CatPicturesCache);
            CachedPictureCount = 0;
        }

        private async Task FillPictureCache()
        {
            List<Stream> picturesToCache = new List<Stream>();

            for (int i = 0; i < MaxCachedItemCount; i++)
            {
                picturesToCache.Add(await GetCatPictureAsync());
            }

            _pictureCache.ReplacePictures(picturesToCache, CatPicturesCache);
        }
    }
}
