using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace PictureAPIs.PictureAPIs
{
    public static class CatPicture
    {

        public static async Task<Stream> GetCatGif(HttpClient _http)
        {
            var response = await _http.GetAsync("https://cataas.com/cat/gif");
            return await response.Content.ReadAsStreamAsync();
        }

        public static async Task<Stream> GetCat(HttpClient _http)
        {
            var response = await _http.GetAsync($"https://cataas.com/cat");
            return await response.Content.ReadAsStreamAsync();
        }
    }
}
