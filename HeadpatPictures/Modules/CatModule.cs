using HeadpatPictures.Contracts;

using System.Threading.Tasks;
using System.IO;
using System;

namespace HeadpatPictures.Modules
{
    public class CatModule : ICatModule
    {
        private readonly ICatService _serv;

        public CatModule(ICatService serv)
        {
            _serv = serv;
        }

        public async Task<Stream> CatPictureAsync(int textSize, string color, string text)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew(); // the code that you want to measure comes here watch.Stop(); var elapsedMs = watch.ElapsedMilliseconds;
            var stream = await _serv.ReturnCachedOrNormalPicture(text);
            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        public async Task<Stream> CatGifAsync()
        {
            var stream = await _serv.GetCatGifAsync();
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }
    }
}
