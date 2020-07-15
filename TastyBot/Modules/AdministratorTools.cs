using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using Interfaces.Contracts.BusinessLogicLayer;
using Interfaces.Entities.Models;
using Enums.UserPermissions;
using System.Collections.Generic;
using System;
using System.Linq;
using Interfaces.Entities.ViewModels;

namespace DiscordUI.Modules
{
    [Name("Administrator Tools")]
    public class AdministratorTools : ModuleBase<SocketCommandContext>
    {
        private readonly IUserService _serv;

        public AdministratorTools(IUserService serv)
        {
            _serv = serv;
        }

        [Command("userdelete")]
        public async Task DeleteUser(IUser userToDelete)
        {
            UserVM sender = new UserVM(_serv.ByDiscordId(Context.User.Id));
            UserVM receiver = new UserVM(_serv.ByDiscordId(userToDelete.Id));

            if (!await CheckAdministrator(sender)) return;
            if (receiver.Permissions.Contains(Permissions.DBUsersUneditableByOthers) && sender.DiscordId != receiver.DiscordId)
            {
                await ReplyAsync($"Cannot edit user ({receiver.Name}){receiver.DiscordId} with permissions: 'DBUsersUneditableByOthers'");
                return;
            }

            if (DeleteUser(receiver))
            {
                await ReplyAsync($"User {receiver.DiscordId} has been removed from the database.");
            }
            else
            {
                await ReplyAsync($"User {receiver.DiscordId} could not be removed.");
            }
        }

        [Command("userdelete")]
        public async Task DeleteUser(ulong discordId)
        {
            UserVM sender = new UserVM(_serv.ByDiscordId(Context.User.Id));
            UserVM receiver = new UserVM(_serv.ByDiscordId(discordId));

            if (!await CheckAdministrator(sender)) return;
            if (receiver.Permissions.Contains(Permissions.DBUsersUneditableByOthers) && sender.DiscordId != receiver.DiscordId)
            {
                await ReplyAsync($"Cannot edit user ({receiver.Name}){receiver.DiscordId} with permissions: 'DBUsersUneditableByOthers'");
                return;
            }
            

            if (DeleteUser(receiver))
            {
                await ReplyAsync($"User {receiver.DiscordId} has been removed from the database.");
            }
            else
            {
                await ReplyAsync($"User {receiver.DiscordId} could not be removed.");
            }
        }

        [Command("dbuserinfo")]
        [Summary("Get all updatable fields for User")]
        public async Task GetUserFieldInfo()
        {
            UserVM sender = new UserVM(_serv.ByDiscordId(Context.User.Id));
            if (!await CheckAdministrator(sender)) return;

            await ReplyAsync(GetUserInfo());
        }

        [Command("dbuserinfo")]
        [Summary("Get values of properties for specified user")]
        public async Task GetUserFieldInfo(IUser user)
        {
            UserVM sender = new UserVM(_serv.ByDiscordId(Context.User.Id));
            UserVM receiver = new UserVM(_serv.ByDiscordId(user.Id));
            if (!await CheckAdministrator(sender)) return;

            await ReplyAsync(GetUserInfo(receiver));
        }

        [Command("dbuserinfo")]
        [Summary("Get values of properties for specified user")]
        public async Task GetUserFieldInfo(ulong DiscordId)
        {
            UserVM sender = new UserVM(_serv.ByDiscordId(Context.User.Id));
            UserVM receiver = new UserVM(_serv.ByDiscordId(DiscordId));
            if (!await CheckAdministrator(sender)) return;

            await ReplyAsync(GetUserInfo(receiver));
        }

        [Command("dbuserpermissionsinfo")]
        public async Task GetUserPermissionsInfo()
        {
            UserVM sender = new UserVM(_serv.ByDiscordId(Context.User.Id));
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

            UserVM sender = new UserVM(_serv.ByDiscordId(Context.User.Id));
            UserVM receiver = new UserVM(_serv.ByDiscordId(userToUpdate.Id));

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

            UserVM sender = new UserVM(_serv.ByDiscordId(Context.User.Id));
            UserVM receiver = new UserVM(_serv.ByDiscordId(discordId));

            if (!await CheckAdministrator(sender) || !await CheckRequiredPermissions(sender, requiredPermissions)) return;
            if (receiver.Permissions.Contains(Permissions.DBUsersUneditableByOthers) && sender.DiscordId != receiver.DiscordId)
            {
                await ReplyAsync($"Cannot edit user ({receiver.Name}){receiver.DiscordId} with permissions: 'DBUsersUneditableByOthers'");
                return;
            }

            await ReplyAsync(UpdateUser(receiver, key, value));
        }

        private string GetUserInfo(UserVM user = null)
        {
            string output = "User fields: \n";
            int count = 1;

            foreach (var prop in typeof(UserVM).GetProperties())
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

        private bool DeleteUser(UserVM receiver)
        {
            User userToDelete = new User()
            {
                Id = receiver.Id,
                Name = receiver.Name,
                DiscordId = receiver.DiscordId,
                Administrator = receiver.Administrator,
                Permissions = receiver.Permissions
            };

            return _serv.Delete(userToDelete);
        }

        private string UpdateUser(UserVM receiver, string key, string value)
        {
            string propertyName = key.Substring(0, 1).ToUpper() + key.Substring(1);
            User userToUpdate = new User() {
                Id = receiver.Id, 
                Name = receiver.Name, 
                DiscordId = receiver.DiscordId, 
                Administrator = receiver.Administrator, 
                Permissions = receiver.Permissions 
            };

            switch (key.ToLower())
            {
                case "id":
                    return "Cannot change field 'Id'";
                case "name":
                    UpdateUserName(userToUpdate, value);
                    return $"User ( {receiver.Name} ){ receiver.DiscordId }.{ propertyName } changed to '{ value }'.";
                case "discordid":
                    return "Cannot change field 'DiscordId'";
                case "administrator":
                    UpdateUserAdministrator(userToUpdate, value);
                    return $"User ( {receiver.Name} ){ receiver.DiscordId }.{ propertyName } changed to '{ bool.Parse(value) }'.";
                case "permissions":
                    if(!UpdateUserPermissions(userToUpdate, value))
                    {
                        return "Use format !userupdate [user/discordid] permissions [add/remove] [permission] (!dbuserpermissionsinfo).";
                    }
                    return $"User ({ userToUpdate.Name }){ userToUpdate.DiscordId }.{ propertyName } permission '{ value.Split(' ')[1] }' updated.";
                default:
                    return "Invalid user field (!dbuserinfo)";
            }
        }

        private void UpdateUserName(User userToUpdate, string value)
        {
            userToUpdate.Name = value;
            _serv.Update(userToUpdate);
        }

        private void UpdateUserAdministrator(User userToUpdate, string value)
        {
            userToUpdate.Administrator = bool.Parse(value);
            _serv.Update(userToUpdate);
        }

        private bool UpdateUserPermissions(User userToUpdate, string value)
        {
            string addOrRemove = value.Split(' ')[0].ToLower();
            List<Permissions> userPermissions = userToUpdate.Permissions;
            if (addOrRemove == "add")
            {
                if (!Enum.TryParse(value.Split(' ')[1], out Permissions permissionToAdd))
                {
                    return false;
                }
                userPermissions.Add(permissionToAdd);
            }
            else if (addOrRemove == "remove")
            {
                if (!Enum.TryParse(value.Split(' ')[1], out Permissions permissionToRemove))
                {
                    return false;
                }
                userPermissions.Remove(permissionToRemove);
            }
            else
            {
                return false;
            }
            _serv.Update(userToUpdate);
            return true;
        }

        private async Task<bool> CheckAdministrator(UserVM user)
        {
            if (user != null && !user.Administrator)
            {
                await ReplyAsync("Unauthorized to use this command.");
                return false;
            }
            return true;
        }

        private async Task<bool> CheckRequiredPermissions(UserVM sender, List<Permissions> requiredPermissions)
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
