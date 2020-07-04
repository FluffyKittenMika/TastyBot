using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Enums.PictureServices;

namespace TastyBot.Contracts
{
    public interface IPictureService
    {
        Task<Stream> GetCatGifAsync();
        Task<Stream> GetCatPictureAsync(string Text, string Color, int Size);
        Task<Stream> GetRegularNekoClientPictureAsync(RegularNekos Type, string Text);
    }

}