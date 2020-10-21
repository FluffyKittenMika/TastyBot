using MusicPlayer.Contracts;
using System;
using VideoLibrary;
using System.Threading.Tasks;

namespace MusicPlayer
{
    public class MusicPlayerModule : IMusicPlayerModule
    {
        public async Task GetYtVideo(string link)
        {
            YouTube youTube = YouTube.Default;
            YouTubeVideo MusicVideo;
            try
            {
                MusicVideo = youTube.GetVideo(link);
            }
            catch (Exception e)
            {

            }
            
        }
        public void PlaySong()
        {
            
        }
    }
}
