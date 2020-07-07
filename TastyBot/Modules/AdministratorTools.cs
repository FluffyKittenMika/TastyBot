using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using Interfaces.Contracts.BusinessLogicLayer;
using Interfaces.Entities.Models;

namespace TastyBot.Modules
{
    [Name("Administrator Tools")]
    public class AdministratorTools : ModuleBase<SocketCommandContext>
    {
        private readonly IUserRepository _repo;

        public AdministratorTools(IUserRepository repo)
        {
            _repo = repo;
        }

        [Command("delete")]
        public async Task DeleteUser(IUser userToDelete)
        {
            User sender = _repo.ByDiscordId(Context.User.Id);
            User receiver = _repo.ByDiscordId(userToDelete.Id);
            if (!IsAdministrator(sender))
            {
                await ReplyAsync("Unauthorized to use this command.");
            }
            else
            {
                bool deleted = _repo.Delete(receiver);
                if (deleted)
                    await ReplyAsync($"User {receiver.DiscordId} has been removed from the database.");
                else
                    await ReplyAsync($"User {receiver.DiscordId} could not be removed.");
            }
        }

        [Command("deleteByDID")]
        public async Task DeleteUser(ulong discordId)
        {
            User sender = _repo.ByDiscordId(Context.User.Id);
            User receiver = _repo.ByDiscordId(discordId);
            if (!IsAdministrator(sender))
            {
                await ReplyAsync("Unauthorized to use this command.");
            }
            else
            {
                bool deleted = _repo.Delete(receiver);
                if (deleted)
                    await ReplyAsync($"User {receiver.DiscordId} has been removed from the database.");
                else
                    await ReplyAsync($"User {receiver.DiscordId} could not be removed.");
            }
        }

        private bool IsAdministrator(User user)
        {
            return (user != null && user.Administrator);
        }
    }
}
