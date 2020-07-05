using System.IO;

namespace HeadpatPictures.Contracts
{
    public interface ITextStreamWriter
    {
        Stream WriteOnStream(Stream stream, string text = "");
    }
}