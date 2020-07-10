using System.IO;
using System.Threading.Tasks;

namespace Interfaces.Contracts.HeadpatPictures
{
    public interface IPictureAPIHub
    {
        Task<Stream> GetStreamByPictureTypeName(string pictureTypeName, object[] optionalArgs = null);
    }
}