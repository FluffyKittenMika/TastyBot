using TastyBot.Extensions;
using TastyBot.Utility;

using Utilities.LoggingService;

using System;
using System.Threading.Tasks;
using System.IO;

using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Utilities.RainbowUtilities;

namespace TastyBot.Services
{
    class Startup
    {
        private readonly Config _botconfig;

        public Startup()
        {
            string time = DateTime.UtcNow.ToString("hh:mm:ss");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Starting up bot...");
            try
            {
                Console.WriteLine($"{time} [StartUp - Info] Base Directory: {AppContext.BaseDirectory}");
                _botconfig = JsonConvert.DeserializeObject<Config>(File.ReadAllText(AppContext.BaseDirectory + "config.json"));
            }
            catch (Exception)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"{time} [StartUp - Critical] No configuration file found, please create one, or the bot simply will not work.");
            }
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"{time} [StartUp - Info] Prefix: '{_botconfig.Prefix}'");
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

        private async Task RunAsync()
        {
            var services = new ServiceCollection();                                 // Create a new instance of a service collection
            ConfigureServices(services);

            var provider = services.BuildServiceProvider();                         // Build the service provider
            provider.GetRequiredService<CommandHandlingService>();                  // Start the command handler service

            await Logging.LogReadyMessage(typeof(Logging));
            await Task.Run(() => Logging.LogRainbowMessage("RainbowUtilities", "Ready"));

            await provider.GetRequiredService<StartupService>().StartAsync();       // Start the startup service
            await Task.Delay(-1);                                                   // Keep the program alive
        }

        private void ConfigureServices(IServiceCollection services)
        {
            #region Discord
            services.ConfigureDiscordSocketClient();
            services.ConfigureCommandService();
            services.ConfigureBotConfig(_botconfig);
            #endregion

            #region TastyBot
            services.ConfigureCommandHandlingService();
            services.ConfigureStartupService();
            #endregion

            #region BusinessLogicLayer
            services.ConfigureUserRepository();
            #endregion

            #region DataAccessLayer
            services.ConfigureUserContext();
            #endregion

            #region Database
            services.ConfigureLiteDB();
            #endregion

            #region HeadpatPictures
            services.ConfigureCatModule();
            services.ConfigureCatService();
            services.ConfigureNekoClientModule();
            services.ConfigureNekoClientService();
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

            #region Mastermind
            services.ConfigureMasterMindModule();
            #endregion
        }
    }
}