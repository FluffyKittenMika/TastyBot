using HeadpatPictures.Utilities.PictureAccess;
using HeadpatPictures.Utilities;
using HeadpatPictures.Contracts;

using Utilities.LoggingService;

using Enums.PictureServices;

using System;
using System.IO;
using System.Threading.Tasks;
using Utilities.TasksManager;

namespace HeadpatPictures.Services
{
    public class PictureHub : IPictureHub
    {
        public async Task<Stream> GetPictureAsync<T>(T pictureTypeSubEnum)
        {
            PictureTypes pictureType = PictureTypesUtilities.GetPictureTypeFromSubEnum(pictureTypeSubEnum);

            Stream stream = pictureType switch
            {
                PictureTypes.ActionNekos => await NekoClient.GetActionNekoClientGifAsync(Enum.Parse<ActionNekos>(pictureTypeSubEnum.ToString())),
                PictureTypes.AnimatedNekos => await NekoClient.SFWNekoClientGifAsync(Enum.Parse<AnimatedNekos>(pictureTypeSubEnum.ToString())),
                PictureTypes.AnimatedNSFWNekos => await NekoClient.NSFWNekoClientGifAsync(Enum.Parse<AnimatedNSFWNekos>(pictureTypeSubEnum.ToString())),
                PictureTypes.NSFWNekos => await NekoClient.GetNSFWNekoClientPictureAsync(Enum.Parse<NSFWNekos>(pictureTypeSubEnum.ToString())),
                PictureTypes.RegularNekos => await NekoClient.GetSFWNekoClientPictureAsync(Enum.Parse<RegularNekos>(pictureTypeSubEnum.ToString())),
                PictureTypes.RegularCats => await CatPicture.GetCatPictureAsync(),
                PictureTypes.AnimatedCats => await CatPicture.GetCatGifAsync(),
                _ => null,
            };

            if (stream == null)
            {
                Logging.LogErrorMessage(typeof(PictureHub).Name, $"Default PictureType triggered, Please add this new PictureTypesValue to the PictureHub switch.").PerformAsyncTaskWithoutAwait();
                throw new NotImplementedException();
            }
            return stream;
        }
    }
}
