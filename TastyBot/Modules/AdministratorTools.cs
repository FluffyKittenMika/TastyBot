using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using Interfaces.Contracts.BusinessLogicLayer;
using Interfaces.Entities.Models;
using Utilities.LoggingService;
using Enums.UserPermissions;
using System.Collections.Generic;
using System;
using System.Linq;

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
            if (receiver.Permissions.Contains(Permissions.DBUsersUneditableByOthers) && sender.DiscordId != receiver.DiscordId)
            {
                await ReplyAsync($"Cannot edit user ({receiver.Name}){receiver.DiscordId} with permissions: 'DBUsersUneditableByOthers'");
                return;
            }

            bool deleted = _repo.Delete(receiver);
            if (deleted)
                await ReplyAsync($"User {receiver.DiscordId} has been removed from the database.");
            else
                await ReplyAsync($"User {receiver.DiscordId} could not be removed.");
        }

        [Command("userdelete")]
        public async Task DeleteUser(ulong discordId)
        {
            User sender = _repo.ByDiscordId(Context.User.Id);
            User receiver = _repo.ByDiscordId(discordId);

            if (!await CheckAdministrator(sender)) return;
            if (receiver.Permissions.Contains(Permissions.DBUsersUneditableByOthers) && sender.DiscordId != receiver.DiscordId)
            {
                await ReplyAsync($"Cannot edit user ({receiver.Name}){receiver.DiscordId} with permissions: 'DBUsersUneditableByOthers'");
                return;
            }

            bool deleted = _repo.Delete(receiver);
            if (deleted)
                await ReplyAsync($"User {receiver.DiscordId} has been removed from the database.");
            else
                await ReplyAsync($"User {receiver.DiscordId} could not be removed.");
        }

        [Command("dbuserinfo")]
        [Summary("Get all updatable fields for User")]
        public async Task GetUserFieldInfo()
        {
            User sender = _repo.ByDiscordId(Context.User.Id);
            if (!await CheckAdministrator(sender)) return;

            await ReplyAsync(GetUserInfo());
        }

        [Command("dbuserinfo")]
        [Summary("Get values of properties for specified user")]
        public async Task GetUserFieldInfo(IUser user)
        {
            User sender = _repo.ByDiscordId(Context.User.Id);
            User receiver = _repo.ByDiscordId(user.Id);
            if (!await CheckAdministrator(sender)) return;

            await ReplyAsync(GetUserInfo(receiver));
        }

        [Command("dbuserinfo")]
        [Summary("Get values of properties for specified user")]
        public async Task GetUserFieldInfo(ulong DiscordId)
        {
            User sender = _repo.ByDiscordId(Context.User.Id);
            User receiver = _repo.ByDiscordId(DiscordId);
            if (!await CheckAdministrator(sender)) return;

            await ReplyAsync(GetUserInfo(receiver));
        }

        [Command("dbuserpermissionsinfo")]
        public async Task GetUserPermissionsInfo()
        {
            User sender = _repo.ByDiscordId(Context.User.Id);
            if (!await CheckAdministrator(sender)) return;

            string output = "Permissions: \n";
            int count = 1;

            foreach (var permission in Enum.GetNames(typeof(Permissions)))
            {
                output += $"{count}. {permission}\n";
                count++;
            }

            await ReplyAsync(output);
        }

        [Command("userupdate")]
        [Summary("Used to update user, use format !userupdate IUser [user field (!dbuserinfo)] [add/remove] [permission]")]
        public async Task UpdateUser(IUser userToUpdate, string key, [Remainder]string value)
        {
            List<Permissions> requiredPermissions = new List<Permissions>();
            if (key.ToLower() == "administrator")
                requiredPermissions.Add(Permissions.DBUsersUpdateAdministrator);
            else if (key.ToLower() == "permissions")
                requiredPermissions.Add(Permissions.DBUsersUpdatePermissions);

            User sender = _repo.ByDiscordId(Context.User.Id);
            User receiver = _repo.ByDiscordId(userToUpdate.Id);

            if (!await CheckAdministrator(sender) || !await CheckRequiredPermissions(sender, requiredPermissions)) return;
            if (receiver.Permissions.Contains(Permissions.DBUsersUneditableByOthers) && sender.DiscordId != receiver.DiscordId)
            {
                await ReplyAsync($"Cannot edit user ({receiver.Name}){receiver.DiscordId} with permissions: 'DBUsersUneditableByOthers'");
                return;
            }

            await ReplyAsync(UpdateUser(receiver, key, value));
        }

        [Command("userupdate")]
        [Summary("Used to update user, use format !userupdate DiscordId [user field (!dbuserinfo)] [add/remove] [permission]")]
        public async Task UpdateUserByDID(ulong discordId, string key, [Remainder] string value)
        {
            List<Permissions> requiredPermissions = new List<Permissions>();
            if (key.ToLower() == "administrator")
                requiredPermissions.Add(Permissions.DBUsersUpdateAdministrator);
            else if (key.ToLower() == "permissions")
                requiredPermissions.Add(Permissions.DBUsersUpdatePermissions);

            User sender = _repo.ByDiscordId(Context.User.Id);
            User receiver = _repo.ByDiscordId(discordId);

            if (!await CheckAdministrator(sender) || !await CheckRequiredPermissions(sender, requiredPermissions)) return;
            if (receiver.Permissions.Contains(Permissions.DBUsersUneditableByOthers) && sender.DiscordId != receiver.DiscordId)
            {
                await ReplyAsync($"Cannot edit user ({receiver.Name}){receiver.DiscordId} with permissions: 'DBUsersUneditableByOthers'");
                return;
            }

            await ReplyAsync(UpdateUser(receiver, key, value));
        }

        private string GetUserInfo(User user = null)
        {
            string output = "User fields: \n";
            int count = 1;

            foreach (var prop in typeof(User).GetProperties())
            {
                if(user == null)
                    if (prop.Name == "Id" || prop.Name == "DiscordId") continue;

                output += $"{count}. {prop.Name}";
                if(user != null)
                {
                    output += ": ";
                    if(prop.PropertyType != typeof(List<Permissions>))
                        output += prop.GetValue(user);
                    else
                    {
                        List<Permissions> permissions = (List<Permissions>)prop.GetValue(user);

                        output += "-";
                        foreach(var permission in permissions.Select(x => x.ToString()).ToList())
                        {
                            output += $" {permission} -";
                        }
                    }
                }
                output += "\n";
                count++;
            }

            return output;
        }

        private string UpdateUser(User receiver, string key, string value)
        {
            string propertyName = key.Substring(0, 1).ToUpper() + key.Substring(1);
            switch (key.ToLower())
            {
                case "id":
                    return "Cannot change field 'Id'";
                case "name":
                    _repo.Update(new User(receiver.Id, value, receiver.DiscordId, receiver.Administrator, receiver.Permissions));
                    return  $"User ({receiver.Name}){receiver.DiscordId}.{propertyName} changed to '{value}'";
                case "discordid":
                    return "Cannot change field 'DiscordId'";
                case "administrator":
                    bool changeAdminTo = bool.Parse(value);
                    _repo.Update(new User(receiver.Id, receiver.Name, receiver.DiscordId, changeAdminTo, receiver.Permissions));
                    return $"User ({receiver.Name}){receiver.DiscordId}.{propertyName} changed to '{changeAdminTo}'";
                case "permissions":
                    string addOrRemove = value.Split(' ')[0].ToLower();
                    List<Permissions> userPermissions = receiver.Permissions;
                    if (addOrRemove == "add")
                    {
                        if (!Enum.TryParse(value.Split(' ')[1], out Permissions permissionToAdd))
                        {
                            return "Invalid user permissions, please check !dbuserpermissionsinfo";
                        }
                        userPermissions.Add(permissionToAdd);
                    }
                    else if (addOrRemove == "remove")
                    {
                        if (!Enum.TryParse(value.Split(' ')[1], out Permissions permissionToRemove))
                        {
                            return "Invalid user permissions, please check !dbuserpermissionsinfo";
                        }
                        userPermissions.Remove(permissionToRemove);
                    }
                    else
                    {
                        return "Use format !userupdate [user/discordid] permissions [add/remove] [permission]";
                    }
                    _repo.Update(new User(receiver.Id, receiver.Name, receiver.DiscordId, receiver.Administrator, userPermissions));
                    string addedOrRemoved = (addOrRemove == "add") ? "added" : "removed" ;
                    return $"User ({receiver.Name}){receiver.DiscordId}.{propertyName} {addedOrRemoved} permission '{value.Split(' ')[1]}'";
                default:
                    return "Invalid user field";
            }
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

        private async Task<bool> CheckRequiredPermissions(User sender, List<Permissions> requiredPermissions)
        {
            foreach (var requiredPermission in requiredPermissions)
            {
                if (!sender.Permissions.Contains(requiredPermission))
                {
                    await ReplyAsync("Unauthorized to use this command.");
                    return false;
                }
            }
            return true;
        }
    }
}
