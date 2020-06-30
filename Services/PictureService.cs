using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace TastyBot.Services
{
    public class PictureService
    {
        private readonly HttpClient _http;

        public PictureService(HttpClient http)
            => _http = http;

        public async Task<Stream> GetCatGifAsync()
        {
            var resp = await _http.GetAsync("https://cataas.com/cat/gif");
            return await resp.Content.ReadAsStreamAsync();
        }
        public async Task<Stream> GetCatPictureAsync(string Text = "", string Color = "white", int Size = 24)
        {
            var resp = await _http.GetAsync($"https://cataas.com/cat/says/" + Text + "?size=" + Size + "&color=" + Color);
            return await resp.Content.ReadAsStreamAsync();
        }
    }
}
