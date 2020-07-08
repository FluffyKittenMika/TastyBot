using Enums.PictureServices;
using Utilities.LoggingService;
using System;

namespace HeadpatPictures.Utilities
{
    public static class PictureEnumUtilities
    {
        public static PictureTypes GetPictureTypeFromEnum<T>(T enumeration)
        {
            if(!Enum.TryParse(enumeration.GetType().Name, out PictureTypes pictureType))
            {
                Logging.LogErrorMessage("PictureEnumUtilities", $"Unable to get a PictureType from: '{enumeration}'");
            }
            return pictureType;
        }

        public static bool IsGif<T>(T enumeration)
        {
            PictureTypes pictureType = GetPictureTypeFromEnum(enumeration);
            return pictureType switch
            {
                PictureTypes.ActionNekos => true,
                PictureTypes.AnimatedNekos => true,
                PictureTypes.AnimatedNSFWNekos => true,
                _ => false,
            };
        }
    }
}