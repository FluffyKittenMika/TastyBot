using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Interfaces.Contracts.BusinessLogicLayer;
using Interfaces.Entities.Models;
using TastyBot.Utility;
using Utilities.LoggingService;

namespace TastyBot.Modules
{
    [Name("Help")]
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        private readonly CommandService _service;
        private readonly Config _config;
        private readonly IUserRepository _repo;

        public HelpModule(CommandService service, Config config, IUserRepository repo)
        {
            _service = service;
            _config = config;
            _repo = repo;

            Logging.LogReadyMessage(this);
        }

        [Command("help")]
        [Summary("Gets help for specified command, or lists all commands")]
        public async Task HelpAsync()
        {
            string prefix = _config.Prefix;
            var builder = new EmbedBuilder()
            {
                Color = new Color(114, 137, 218),
                Description = "These are the commands you can use"
            };

            foreach (var module in _service.Modules)
            {
                
                if (module.Name == "Administrator Tools")
                {
                    User user = _repo.ByDiscordId(Context.User.Id);
                    if (user != null && !user.Administrator) continue;
                }

                string description = null;
                foreach (var cmd in module.Commands.GroupBy(x => x.Name).Select(x => x.FirstOrDefault()).ToList())
                {
                    var result = await cmd.CheckPreconditionsAsync(Context);
                    if (result.IsSuccess)
                        description += $"{prefix}{cmd.Aliases.First()}\n";
                }

                if (!string.IsNullOrWhiteSpace(description))
                {
                    builder.AddField(x =>
                    {
                        x.Name = module.Name;
                        x.Value = description;
                        x.IsInline = false;
                    });
                }
            }

            await ReplyAsync("", false, builder.Build());
        }

        [Command("help")]
        [Summary("Gets help for specified command, or lists all commands")]
        public async Task HelpAsync(string command)
        {
            var result = _service.Search(Context, command);

            if (!result.IsSuccess)
            {
                await ReplyAsync($"Sorry, I couldn't find a command like **{command}**.");
                return;
            }

            string prefix = _config.Prefix;
            var builder = new EmbedBuilder()
            {
                Color = new Color(114, 137, 218),
                Description = $"Here are some commands like **{command}**"
            };

            foreach (var match in result.Commands)
            {
                var cmd = match.Command;

                builder.AddField(x =>
                {
                    x.Name = string.Join(", ", cmd.Aliases);
                    x.Value = $"Parameters: {string.Join(", ", cmd.Parameters.Select(p => p.Name))}\n" +
                              $"Summary: {cmd.Summary}";
                    x.IsInline = false;
                });
            }

            await ReplyAsync("", false, builder.Build());
        }
    }
}
