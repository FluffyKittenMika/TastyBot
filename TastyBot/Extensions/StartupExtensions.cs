using Authorization.Contracts;
using Authorization.HelperClasses;
using Authorization.Users;

using FileManager.Contracts;

using FutureHeadPats.Contracts;
using FutureHeadPats.Modules;
using FutureHeadPats.Services;
using FutureHeadPats.HelperClasses;

using HeadpatDungeon.Contracts;
using HeadpatDungeon.Containers;
using HeadpatDungeon.Strategies;

using TastyBot.Contracts;
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

        #region TastyBot

        public static void ConfigureCommandHandlingService(this IServiceCollection services)
        {
            services.AddScoped<ICommandHandlingService, CommandHandlingService>(); // Add the Command handler to the collection
        }

        public static void ConfigureLoggingService(this IServiceCollection services)
        {
            services.AddSingleton<LoggingService>();         // Add loggingservice to the collection
        }

        public static void ConfigureStartupService(this IServiceCollection services)
        {
            services.AddScoped<IStartupService, StartupService>();         // Add startupservice to the collection
        }

        public static void ConfigurePictureService(this IServiceCollection services)
        {
            services.AddScoped<IPictureService, PictureService>();         // Add the picture service, it depends on HTTP
        }
        public static void ConfigureRainbowService(this IServiceCollection services)
        {
            services.AddSingleton<RainbowService>();         // Add Rainbow Service, not sure if it needs to be one
        }
        public static void ConfigureBotcatService(this IServiceCollection services)
        {
            services.AddSingleton<BotCatService>();
        }

        public static void ConfigureBotConfig(this IServiceCollection services, Config botConfig)
        {
            services.AddSingleton(botConfig);				// Add the configuration to the collection
        }


        #endregion

        #region Authorization

        public static void ConfigurePermissionHandler(this IServiceCollection services)
        {
            services.AddScoped<IPermissionHandler, PermissionHandler>();
        }

        public static void ConfigureUsersContainer(this IServiceCollection services)
        {
            services.AddScoped<IUsersContainer, UsersContainer>();
        }

        #endregion

        #region FileManager

        public static void ConfigureFileManager(this IServiceCollection services)
        {
            services.AddScoped<IFileManager, FileManager.FileManager>();
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