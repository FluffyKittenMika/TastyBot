using Enums.UserPermissions;
using LiteDB;
using System.Collections.Generic;

namespace Interfaces.Entities.Models
{
    public class User
    {
        [BsonId]
        public int Id { get; }
        public string Name { get; }
        public ulong DiscordId { get; }
        public bool Administrator { get; }
        public List<Permissions> Permissions { get; }

        public User(int id, string name, ulong discordId, bool administrator, List<Permissions> permissions)
        {
            Name = name;
            DiscordId = discordId;
            Administrator = administrator;
            Permissions = permissions;
            Id = id;
        }

        public bool HasPermission(Permissions permission)
        {
            return Permissions.Contains(permission);
        }
    }
}
