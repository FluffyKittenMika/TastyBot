using Enums.PictureServices;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace HeadpatPictures.Utilities
{
    public static class CatPicture
    {
        private static readonly HttpClient _http = new HttpClient();

        private static async Task<HttpResponseMessage> GetCatGifAsync()
        {
            return await _http.GetAsync("https://cataas.com/cat/gif");
        }

        private static async Task<HttpResponseMessage> GetCatPictureAsync()
        {
            return await _http.GetAsync($"https://cataas.com/cat");
            
        }

        public static async Task<Stream> GetCatItem(CatItems catEnum)
        {
            HttpResponseMessage response = catEnum switch
            {
                CatItems.Picture => await GetCatPictureAsync(),
                CatItems.Gif => await GetCatGifAsync(),
                _ => null,
            };

            return await response.Content.ReadAsStreamAsync();
        }
    }
}
