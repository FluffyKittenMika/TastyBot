using Enums.UserPermissions;
using System.Collections.Generic;

namespace Interfaces.Entities.Models
{
    public class UserCreate
    {
        public string Name { get; set; }
        public ulong DiscordId { get; set; }
        public bool Administrator { get; set; }
        public List<Permissions> Permissions { get; set; }
    }
}
