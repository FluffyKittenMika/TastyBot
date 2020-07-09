using Interfaces.Contracts.BusinessLogicLayer;
using Interfaces.Contracts.DataAccessLayer;
using Interfaces.Contracts.Database;
using Interfaces.Contracts.HeadpatPictures;

using HeadpatPictures.Contracts;
using HeadpatPictures.Modules;
using HeadpatPictures.Services;
using HeadpatPictures.Utilities;

using FutureHeadPats.Contracts;
using FutureHeadPats.Modules;
using FutureHeadPats.Services;
using FutureHeadPats.HelperClasses;

using HeadpatDungeon.Contracts;
using HeadpatDungeon.Containers;
using HeadpatDungeon.Strategies;

using BusinessLogicLayer.Repositories;
using DataAccessLayer.Context;

using TastyBot.Services;
using TastyBot.Utility;

using MasterMind.Contracts;
using MasterMind.Modules;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Microsoft.Extensions.DependencyInjection;
using Utilities.LoggingService;

namespace TastyBot.Extensions
{
    // Logging.LogInfoMessage("Namespace", "Ready");
    public static class StartupExtensions
    {
        #region Discord

        public static void ConfigureDiscord(this IServiceCollection services, Config botConfig)
        {
            services.ConfigureDiscordSocketClient();
            services.ConfigureCommandService();
            services.ConfigureBotConfig(botConfig);
            Logging.LogInfoMessage("Discord", "Ready");
        }

        private static void ConfigureDiscordSocketClient(this IServiceCollection services)
        {
            services.AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
            {                                       // Add discord to the collection
                LogLevel = LogSeverity.Verbose,     // Tell the logger to give Verbose amount of info
                MessageCacheSize = 1000             // Cache 1,000 messages per channel
            }));
        }

        private static void ConfigureCommandService(this IServiceCollection services)
        {
            services.AddSingleton(new CommandService(new CommandServiceConfig
            {                                       // Add the command service to the collection
                LogLevel = LogSeverity.Debug,     // Tell the logger to give Verbose amount of info
                DefaultRunMode = RunMode.Async,     // Force all commands to run async by default
            }));
        }

        private static void ConfigureBotConfig(this IServiceCollection services, Config botConfig)
        {
            services.AddSingleton(botConfig);				// Add the configuration to the collection
        }

        #endregion

        #region TastyBot

        public static void ConfigureTastyBot(this IServiceCollection services)
        {
            services.ConfigureCommandHandlingService();
            services.ConfigureStartupService();
            Logging.LogInfoMessage("TastyBot", "Ready");
        }

        public static void ConfigureCommandHandlingService(this IServiceCollection services)
        {
            services.AddSingleton<CommandHandlingService>();
        }

        public static void ConfigureStartupService(this IServiceCollection services)
        {
            services.AddSingleton<StartupService>();
        }

        #endregion

        #region BusinessLogicLayer

        public static void ConfigureBusinessLogicLayer(this IServiceCollection services)
        {
            services.ConfigureUserRepository();
            Logging.LogInfoMessage("BusinessLogicLayer", "Ready");
        }

        private static void ConfigureUserRepository(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
        }

        #endregion

        #region DataAccessLayer

        public static void ConfigureDataAccessLayer(this IServiceCollection services)
        {
            services.ConfigureUserContext();
            Logging.LogInfoMessage("DataAccessLayer", "Ready");
        }

        private static void ConfigureUserContext(this IServiceCollection services)
        {
            services.AddScoped<IUserContext, UserContext>();
        }

        #endregion

        #region Databases

        public static void ConfigureDatabases(this IServiceCollection services)
        {
            services.ConfigureLiteDB();
            Logging.LogInfoMessage("Databases", "Ready");
        }

        private static void ConfigureLiteDB(this IServiceCollection services)
        {
            services.AddScoped<ILiteDB, Databases.LiteDB>();
        }

        #endregion

        #region HeadpatPictures

        public static void ConfigureHeadpatPictures(this IServiceCollection services)
        {
            services.ConfigurePictureModule();
            services.ConfigurePictureService();
            services.ConfigurePictureHub();
            Logging.LogInfoMessage("HeadpatPictures", "Ready");
        }

        private static void ConfigurePictureModule(this IServiceCollection services)
        {
            services.AddScoped<IPictureModule, PictureModule>();
        }

        private static void ConfigurePictureService(this IServiceCollection services)
        {
            services.AddScoped<IPictureService, PictureService>();
        }

        private static void ConfigurePictureHub(this IServiceCollection services)
        {
            services.AddScoped<IPictureHub, PictureHub>();
        }

        #endregion

        #region FutureHeadPats

        public static void ConfigureFutureHeadPats(this IServiceCollection services)
        {
            services.ConfigureFileManagerFPH();
            services.ConfigureHeadpatService();
            services.ConfigureHeadPatModule();
            Logging.LogInfoMessage("FutureHeadPats", "Ready");
        }

        private static void ConfigureFileManagerFPH(this IServiceCollection services)
        {
            services.AddScoped<IFileManagerFHP, FileManagerFHP>();
        }

        private static void ConfigureHeadpatService(this IServiceCollection services)
        {
            services.AddScoped<IHeadpatService, HeadpatService>();
        }

        private static void ConfigureHeadPatModule(this IServiceCollection services)
        {
            services.AddScoped<IFhpModule, FhpModule>();
        }

        #endregion

        #region HeadpatDungeon

        /*public static void ConfigureEntityContainer(this IServiceCollection services)
        {
            services.AddScoped<IEntityContainer, EntityContainer>();
        }

        public static void ConfigureItemContainer(this IServiceCollection services)
        {
            services.AddScoped<IItemContainer, ItemContainer>();
        }

        public static void ConfigureRecipeContainer(this IServiceCollection services)
        {
            services.AddScoped<IRecipeContainer, RecipeContainer>();
        }

        public static void ConfigureCrafting(this IServiceCollection services)
        {
            services.AddScoped<ICrafting, Crafting>();
        }*/

        #endregion

        #region MasterMind

        public static void ConfigureMasterMind(this IServiceCollection services)
        {
            services.ConfigureMasterMindModule();
            Logging.LogInfoMessage("MasterMind", "Ready");
        }

        private static void ConfigureMasterMindModule(this IServiceCollection services)
        {
            services.AddScoped<IMasterMindModule, MasterMindModule>(); // Add the Command handler to the collection
        }

        #endregion
    }
}