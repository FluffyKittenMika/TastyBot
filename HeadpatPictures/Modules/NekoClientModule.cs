using HeadpatPictures.Contracts;

using Enums.PictureServices;

using System.Threading.Tasks;
using System.IO;
using Utilities.LoggingService;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using NekosSharp;
using HeadpatPictures.Services;

namespace HeadpatPictures.Modules
{
    public class NekoClientModule : INekoClientModule
    {
        private readonly IPictureCacheContainer _pictureCache;
        private readonly ITextStreamWriter _writer;
        private readonly int MaxUniqueCounter = 3;
        private readonly Stopwatch watch;
        private readonly INekoClientService _neko;

        public NekoClientModule(INekoClientService neko, IPictureCacheContainer pictureCache, ITextStreamWriter writer)
        {
            _pictureCache = pictureCache;
            _writer = writer;
            _neko = neko;
            watch = new Stopwatch(); 
        }


        #region Images
        public async Task<Stream> SFWNekoClientPictureAsync(RegularNekos regularNekos, string text = "")
        {
            var stream = await ReturnCacheAction(PictureTypes.RegularNekos, regularNekos);
            stream = _writer.WriteOnStream(stream, text);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        public async Task<Stream> NSFWNekoClientPictureAsync(NSFWNekos unregularNekos, string text = "")
        {
            var stream = await ReturnCacheAction(PictureTypes.NSFWNekos, unregularNekos);
            stream = _writer.WriteOnStream(stream, text);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }
        #endregion

        #region Animated

        public async Task<Stream> ActionNekoClientGifAsync(ActionNekos actionNekos)
        {
            var stream = await ReturnCacheAction(PictureTypes.ActionNekos, actionNekos);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        public async Task<Stream> NSFWNekoClientGifAsync(AnimatedNSFWNekos unregularNekos)
        {
            var stream = await ReturnCacheAction(PictureTypes.AnimatedNSFWNekos, unregularNekos);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        public async Task<Stream> SFWNekoClientGifAsync(AnimatedNekos regularNekos)
        {
            var stream = await ReturnCacheAction(PictureTypes.AnimatedNekos, regularNekos);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        #endregion


        private async Task<Stream> ReturnCacheAction<T>(PictureTypes type, T inputenum) 
        {
            watch.Start();
            Type t = typeof(T);


            //Sets the name to Type + value
            string key = t.Name + inputenum.ToString();

            //If amount of images is equal to 0, or the key does not exist, fill it

            if (_pictureCache.GetCacheCount(key) == 0 || !_pictureCache.Exists(key))
                await FillPictureCache(key,type, inputenum); //Fills it to MaxUniqueCounter
              
            //Get the first image from the cache
            Stream stream = _pictureCache.GetCachedPictures(key).FirstOrDefault();

            //Remove it from the cache, and replace it with a new.
            RemoveCachePicture(key);

            watch.Stop();
            await Logging.LogDebugMessage("Cache", $"Fetching image took: {watch.ElapsedMilliseconds}ms - Key:{key}");

            return stream;
            
        }


        private void RemoveCachePicture(string key)
        {
            List<Stream> pictures = _pictureCache.GetCachedPictures(key);
            pictures.Remove(pictures.FirstOrDefault());
            _pictureCache.ReplacePictures(pictures, key);
        }

        private async Task FillPictureCache<T>(string key, PictureTypes type, T inputtype, string text = "")
        {
            
            List<Stream> picturesToCache = new List<Stream>();

            for (int i = 0; i < MaxUniqueCounter; i++)
            {
                switch (type)   
                {
                    case PictureTypes.ActionNekos:
                            picturesToCache.Add(await _neko.GetActionNekoClientGifAsync(Enum.Parse<ActionNekos>(inputtype.ToString())));
                        break;
                    case PictureTypes.AnimatedNekos:
                            picturesToCache.Add(await _neko.SFWNekoClientGifAsync(Enum.Parse<AnimatedNekos>(inputtype.ToString())));
                        break;
                    case PictureTypes.AnimatedNSFWNekos:
                            picturesToCache.Add(await _neko.NSFWNekoClientGifAsync(Enum.Parse<AnimatedNSFWNekos>(inputtype.ToString())));
                        break;
                    case PictureTypes.NSFWNekos:
                            picturesToCache.Add(await _neko.GetNSFWNekoClientPictureAsync(Enum.Parse<NSFWNekos>(inputtype.ToString()), text));
                        break;
                    case PictureTypes.RegularNekos:
                            picturesToCache.Add(await _neko.GetSFWNekoClientPictureAsync(Enum.Parse<RegularNekos>(inputtype.ToString()), text));
                        break;
                    default:
                        await Logging.LogDebugMessage($"Neko", $"Default Triggered in Cache system Type: {inputtype} Key: {key}");
                        break;
                }

            }


            _pictureCache.ReplacePictures(picturesToCache, key);
        }

    }
}
