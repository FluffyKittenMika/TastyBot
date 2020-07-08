using Enums.PictureServices;

namespace HeadpatPictures.Utilities
{
    public static class CatItemsEnumUtilities
    {
        public static bool IsGif(CatItems enumeration)
        {
            return enumeration switch
            {
                CatItems.Gif => true,
                _ => false,
            };
        }
    }
}
