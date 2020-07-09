using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace HeadpatPictures.Utilities.PictureAccess
{
    public static class CatPicture
    {
        private static readonly HttpClient _http = new HttpClient();

        public static async Task<Stream> GetCatGifAsync()
        {
            var response = await _http.GetAsync("https://cataas.com/cat/gif");
            return await response.Content.ReadAsStreamAsync();
        }

        public static async Task<Stream> GetCatPictureAsync()
        {
            var response = await _http.GetAsync($"https://cataas.com/cat");
            return await response.Content.ReadAsStreamAsync();
        }
    }
}
