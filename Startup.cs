using System;
using System.Net.Http;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TastyBot.Services
{
	class Startup
	{
		public IConfigurationRoot Configuration { get; }

		public Startup(string[] args)
		{
			var builder = new ConfigurationBuilder()        // Create a new instance of the config builder
				.SetBasePath(AppContext.BaseDirectory)      // Specify the default location for the config file
				.AddYamlFile("_config.yml");                // Add this (yaml encoded) file to the configuration
			Configuration = builder.Build();                // Build the configuration
			Console.WriteLine("PREFIX IS: " + Configuration["prefix"]);
		}

		public static async Task RunAsync(string[] args)
		{
			var startup = new Startup(args);
			await startup.RunAsync();
		}

		public async Task RunAsync()
		{
			var services = new ServiceCollection();                                 // Create a new instance of a service collection
			ConfigureServices(services);

			var provider = services.BuildServiceProvider();                         // Build the service provider
			provider.GetRequiredService<LoggingService>();                          // Start the logging service
			provider.GetRequiredService<CommandHandlingService>();                  // Start the command handler service

			await provider.GetRequiredService<StartupService>().StartAsync();       // Start the startup service
			await Task.Delay(-1);                                                   // Keep the program alive
		}

		private void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
			{                                       // Add discord to the collection
				LogLevel = LogSeverity.Verbose,     // Tell the logger to give Verbose amount of info
				MessageCacheSize = 1000             // Cache 1,000 messages per channel
			}))
			.AddSingleton(new CommandService(new CommandServiceConfig
			{                                       // Add the command service to the collection
				LogLevel = LogSeverity.Debug,     // Tell the logger to give Verbose amount of info
				DefaultRunMode = RunMode.Async,     // Force all commands to run async by default
			}))
			.AddSingleton<CommandHandlingService>() // Add the Command handler to the collection
			.AddSingleton<StartupService>()         // Add startupservice to the collection
			.AddSingleton<LoggingService>()         // Add loggingservice to the collection
			.AddSingleton<Random>()                 // Add random to the collection
			.AddSingleton<HttpClient>()             // Add a Http client so we can just keep one ready
			.AddSingleton<PictureService>()         // Add the picture service, it depends on HTTP
			.AddSingleton<HeadpatService>()
			.AddSingleton<RainbowService>()         // Add Rainbow Service, not sure if it needs to be one
			.AddSingleton(Configuration);           // Add the configuration to the collection
		}
	}
}
