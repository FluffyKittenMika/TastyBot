using Enums.PictureServices;

using Utilities.LoggingService;

using System;
using Utilities.TasksManager;

namespace HeadpatPictures.Utilities
{
    public static class PictureTypesUtilities
    {
        public static PictureTypes GetPictureTypeFromSubEnum<T>(T pictureTypesSubEnumValue)
        {
            if(!Enum.TryParse(pictureTypesSubEnumValue.GetType().Name, out PictureTypes pictureType))
            {
                Logging.LogErrorMessage(typeof(PictureTypesUtilities).Name, $"Unable to get a PictureType from: '{pictureTypesSubEnumValue}'").PerformAsyncTaskWithoutAwait();
            }
            return pictureType;
        }

        public static bool PictureTypeIsGif(PictureTypes pictureTypesValue)
        {
            return pictureTypesValue switch
            {
                PictureTypes.ActionNekos => true,
                PictureTypes.AnimatedNekos => true,
                PictureTypes.AnimatedNSFWNekos => true,
                PictureTypes.AnimatedCats => true,
                _ => false,
            };
        }

        public static bool SubEnumIsGif<T>(T pictureTypesSubEnumValue)
        {
            PictureTypes pictureType = GetPictureTypeFromSubEnum(pictureTypesSubEnumValue);
            return PictureTypeIsGif(pictureType);
        }
    }
}