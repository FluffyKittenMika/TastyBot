using System.IO;
using System.Threading.Tasks;

namespace Interfaces.Contracts.HeadpatPictures
{
    public interface IPictureModule
    {
        Task<Stream> GetStreamFromEnumAsync<T>(T nekoClientEnum, string text = "");
    }
}