using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using Interfaces.Contracts.BusinessLogicLayer;
using Interfaces.Entities.Models;
using Utilities.LoggingService;
using Enums.UserPermissions;

namespace TastyBot.Modules
{
    [Name("Administrator Tools")]
    public class AdministratorTools : ModuleBase<SocketCommandContext>
    {
        private readonly IUserRepository _repo;

        public AdministratorTools(IUserRepository repo)
        {
            _repo = repo;

            Logging.LogReadyMessage(this);
        }

        [Command("userdelete")]
        public async Task DeleteUser(IUser userToDelete)
        {
            User sender = _repo.ByDiscordId(Context.User.Id);
            User receiver = _repo.ByDiscordId(userToDelete.Id);
            if (!await CheckAdministrator(sender)) return;

            bool deleted = _repo.Delete(receiver);
            if (deleted)
                await ReplyAsync($"User {receiver.DiscordId} has been removed from the database.");
            else
                await ReplyAsync($"User {receiver.DiscordId} could not be removed.");
        }

        [Command("userdeleteByDID")]
        public async Task DeleteUser(ulong discordId)
        {
            User sender = _repo.ByDiscordId(Context.User.Id);
            User receiver = _repo.ByDiscordId(discordId);
            if (!await CheckAdministrator(sender)) return;

            bool deleted = _repo.Delete(receiver);
            if (deleted)
                await ReplyAsync($"User {receiver.DiscordId} has been removed from the database.");
            else
                await ReplyAsync($"User {receiver.DiscordId} could not be removed.");
        }

        [Command("dbuserinfo")]
        public async Task GetUserFieldInfo()
        {
            User sender = _repo.ByDiscordId(Context.User.Id);
            if (!await CheckAdministrator(sender)) return;

            string output = "User fields: \n";
            int count = 1;

            foreach (var prop in typeof(User).GetProperties())
            {
                if (prop.Name == "Id") continue;
                output += $"{count}. {prop.Name}\n";
                count++;
            }

            await ReplyAsync(output);
        }

        [Command("dbuserpermissionsinfo")]
        public async Task GetUserPermissionsInfo()
        {
            User sender = _repo.ByDiscordId(Context.User.Id);
            if (!await CheckAdministrator(sender)) return;

            string output = "Permissions: \n";
            int count = 1;

            foreach (var prop in typeof(Permissions).GetProperties())
            {
                output += $"{count}. {prop.Name}\n";
                count++;
            }

            await ReplyAsync(output);
        }

        [Command("userupdate")]
        public async Task UpdateUser(IUser userToUpdate, string key, string value)
        {
            User sender = _repo.ByDiscordId(Context.User.Id);
            User receiver = _repo.ByDiscordId(userToUpdate.Id);
            if (!await CheckAdministrator(sender)) return;
            if(key == "Id")
            {
                await ReplyAsync("Cannot change field 'Id'");
                return;
            }

            switch(key)
            {
                case "Id":
                    break;
            }

            User updatedReceiver = new User(receiver.Id, receiver.Name, 1, true, receiver.Permissions);
            _repo.Update(updatedReceiver);
        }

        [Command("userupdateByDID")]
        public async Task UpdateUserByDID(ulong discordId, string key, string value)
        {
            User sender = _repo.ByDiscordId(Context.User.Id);
            User receiver = _repo.ByDiscordId(discordId);
            if (!await CheckAdministrator(sender)) return;
            if (key == "Id")
            {
                await ReplyAsync("Cannot change field 'Id'");
                return;
            }

            User updatedReceiver = new User(receiver.Id, receiver.Name, 1, true, receiver.Permissions);
            _repo.Update(updatedReceiver);
        }

        private async Task<bool> CheckAdministrator(User user)
        {
            if (user != null && !user.Administrator)
            {
                await ReplyAsync("Unauthorized to use this command.");
                return false;
            }
            return true;
        }
    }
}
