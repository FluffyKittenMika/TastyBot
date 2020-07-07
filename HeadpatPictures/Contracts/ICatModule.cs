using System.IO;
using System.Threading.Tasks;

namespace HeadpatPictures.Contracts
{
    public interface ICatModule
    {
        Task<Stream> CatPictureAsync(int textsize, string Colour, string text);
        Task<Stream> CatGifAsync();
    }
}