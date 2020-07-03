using Enums.UserPermissions;
using System.Collections.Generic;

namespace Authorization.Entities
{
    public class User
    {
        public string Name { get; set; }
        public ulong DiscordId { get; set; }
        public bool Administrator { get; set; }
        public List<Permissions> Permissions { get; set; }
    }
}
