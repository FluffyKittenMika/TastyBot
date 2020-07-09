using System.IO;
using System.Threading.Tasks;

namespace HeadpatPictures.Contracts
{
    public interface IPictureHub
    {
        Task<Stream> GetPictureAsync<T>(T pictureTypeSubEnum);
    }
}
