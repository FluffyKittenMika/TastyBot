using Enums.UserPermissions;
using LiteDB;
using System.Collections.Generic;

namespace Interfaces.Entities.Models
{
    public class User
    {
        [BsonId]
        public int Id { get; set;  }
        public string Name { get; set; }
        public ulong DiscordId { get; set; }
        public bool Administrator { get; set;  }
        public List<Permissions> Permissions { get; set; }
    }
}
