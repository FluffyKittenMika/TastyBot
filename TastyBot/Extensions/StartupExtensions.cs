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

using Interfaces.Contracts.BusinessLogicLayer;
using Interfaces.Contracts.DataAccessLayer;
using Interfaces.Contracts.Database;

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

namespace TastyBot.Extensions
{
    public static class StartupExtensions
    {
        #region Discord

        public static void ConfigureDiscordSocketClient(this IServiceCollection services)
        {
            services.AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
            {                                       // Add discord to the collection
                LogLevel = LogSeverity.Verbose,     // Tell the logger to give Verbose amount of info
                MessageCacheSize = 1000             // Cache 1,000 messages per channel
            }));
        }

        public static void ConfigureCommandService(this IServiceCollection services)
        {
            services.AddSingleton(new CommandService(new CommandServiceConfig
            {                                       // Add the command service to the collection
                LogLevel = LogSeverity.Debug,     // Tell the logger to give Verbose amount of info
                DefaultRunMode = RunMode.Async,     // Force all commands to run async by default
            }));
        }

        public static void ConfigureBotConfig(this IServiceCollection services, Config botConfig)
        {
            services.AddSingleton(botConfig);				// Add the configuration to the collection
        }

        #endregion

        #region TastyBot

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

        public static void ConfigureUserRepository(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
        }

        #endregion

        #region DataAccessLayer

        public static void ConfigureUserContext(this IServiceCollection services)
        {
            services.AddScoped<IUserContext, UserContext>();
        }

        #endregion

        #region Database

        public static void ConfigureLiteDB(this IServiceCollection services)
        {
            services.AddScoped<ILiteDB, Database.LiteDB>();
        }

        #endregion

        #region HeadpatPictures

        public static void ConfigureTextStreamWriter(this IServiceCollection services)
        {
            services.AddScoped<ITextStreamWriter, TextStreamWriter>();
        }

        public static void ConfigureCatService(this IServiceCollection services)
        {
            services.AddScoped<ICatService, CatService>();
        }

        public static void ConfigureNekoClientService(this IServiceCollection services)
        {
            services.AddScoped<INekoClientService, NekoClientService>();
        }

        public static void ConfigureCatModule(this IServiceCollection services)
        {
            services.AddScoped<ICatModule, CatModule>();
        }

        public static void ConfigureNekoClientModule(this IServiceCollection services)
        {
            services.AddScoped<INekoClientModule, NekoClientModule>();
        }

        public static void ConfigurePictureCacheContainer(this IServiceCollection services)
        {
            services.AddScoped<IPictureCacheContainer, PictureCacheContainer>();
        }

        #endregion

        #region FutureHeadPats

        public static void ConfigureFileManagerFPH(this IServiceCollection services)
        {
            services.AddScoped<IFileManagerFHP, FileManagerFHP>();
        }

        public static void ConfigureHeadpatService(this IServiceCollection services)
        {
            services.AddScoped<IHeadpatService, HeadpatService>();
        }

        public static void ConfigureHeadPatModule(this IServiceCollection services)
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

        public static void ConfigureMasterMindModule(this IServiceCollection services)
        {
            services.AddScoped<IMasterMindModule, MasterMindModule>(); // Add the Command handler to the collection
        }

        #endregion
    }
}