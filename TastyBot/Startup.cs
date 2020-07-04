using TastyBot.Extensions;

using System;
using System.Threading.Tasks;
using System.IO;

using Microsoft.Extensions.DependencyInjection;
using TastyBot.Utility;
using Newtonsoft.Json;
using TastyBot.Contracts;

namespace TastyBot.Services
{
    class Startup
    {
        public Config Botconfig { get; }
        public Startup()
        {
            //load config
            try
            {
                Botconfig = JsonConvert.DeserializeObject<Config>(File.ReadAllText(AppContext.BaseDirectory + "config.json"));
            }
            catch (Exception)
            {
                Console.WriteLine("No configuration file found, please create one, or the bot simply will not work.");
            }
            Console.WriteLine("PREFIX IS: " + Botconfig.Prefix);
        }

        public static async Task RunAsync(string[] args)
        {
            if (args is null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            var startup = new Startup();
            await startup.RunAsync();
        }

        public async Task RunAsync()
        {
            var services = new ServiceCollection();                                 // Create a new instance of a service collection
            ConfigureServices(services);

            var provider = services.BuildServiceProvider();                         // Build the service provider
            provider.GetRequiredService<LoggingService>();                          // Start the logging service
            provider.GetRequiredService<ICommandHandlingService>();                  // Start the command handler service

            await provider.GetRequiredService<IStartupService>().StartAsync();       // Start the startup service
            await Task.Delay(-1);                                                   // Keep the program alive
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureDiscordSocketClient();
            services.ConfigureCommandService();
            services.ConfigureBotConfig(Botconfig);

            #region TastyBot
            services.ConfigureCommandHandlingService();
            services.ConfigureLoggingService();

            services.ConfigureStartupService();
            services.ConfigureBotcatService();
            services.ConfigurePictureService();
            services.ConfigureRainbowService();
            #endregion

            #region Authorization
            services.ConfigurePermissionHandler();
            services.ConfigureUsersContainer();
            #endregion

            #region FileManager
            services.ConfigureFileManager();
            #endregion

            #region FutureHeadPats
            services.ConfigureFileManagerFPH();
            services.ConfigureHeadpatService();
            services.ConfigureHeadPatModule();
            #endregion

            #region HeadpatDungeon
            /*services.ConfigureEntityContainer();
			services.ConfigureItemContainer();
			services.ConfigureRecipeContainer();*/
            #endregion
        }
    }
}