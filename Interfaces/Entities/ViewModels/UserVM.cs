using Enums.UserPermissions;
using Interfaces.Entities.Models;
using System.Collections.Generic;

namespace Interfaces.Entities.ViewModels
{
    public class UserVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ulong DiscordId { get; set; }
        public bool Administrator { get; set; }
        public List<Permissions> Permissions { get; set; }

        public UserVM(User user)
        {
            Id = user.Id;
            Name = user.Name;
            DiscordId = user.DiscordId;
            Administrator = user.Administrator;
            Permissions = user.Permissions;
        }

        public UserVM()
        {

        }

        public UserVM(int id, string name, ulong discordId, bool administrator, List<Permissions> permissions)
        {
            Id = id;
            Name = name;
            DiscordId = discordId;
            Administrator = administrator;
            Permissions = permissions;
        }

        public bool HasPermission(Permissions permission)
        {
            return Permissions.Contains(permission);
        }
    }
}
