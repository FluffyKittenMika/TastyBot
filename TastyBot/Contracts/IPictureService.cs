using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace TastyBot.Contracts
{
    public interface IPictureService
    {
        Task<Stream> GetCatGifAsync();
        Task<Stream> GetCatPictureAsync(string Text, string Color, int Size);
        Task<Stream> GetNekoPictureAsync(string Text, string Color);
        Stream WriteOnStream(Stream stream, string Text, string col);


    }

}