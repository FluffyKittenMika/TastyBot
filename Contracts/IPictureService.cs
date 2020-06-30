using System.IO;
using System.Threading.Tasks;

namespace TastyBot.Contracts
{
    public interface IPictureService
    {
        Task<Stream> GetCatGifAsync();
        Task<Stream> GetCatPictureAsync();
        Task<Stream> GetCatPictureWTxtAsync(string Text);
        Task<Stream> GetCatPictureWTxtAsyncAndColor(string Text, string Color, int Size);
    }
}