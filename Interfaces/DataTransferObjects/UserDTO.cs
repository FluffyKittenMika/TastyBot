using LiteDB;
using System.Collections.Generic; 

namespace Interfaces.DataTransferObjects
{
    public class UserDTO
    {
        public UserDTO()
        {

        }

        public UserDTO(string name, ulong discordId, bool administrator, List<string> permissions)
        {
            Name = name;
            DiscordId = discordId;
            Administrator = administrator;
            Permissions = permissions;
        }

        public UserDTO(int id, string name, ulong discordId, bool administrator, List<string> permissions)
        {
            Id = id;
            Name = name;
            DiscordId = discordId;
            Administrator = administrator;
            Permissions = permissions;
        }

        [BsonId]
        public int Id { get; set; }
        public string Name { get; set; }
        public ulong DiscordId { get; set; }
        public bool Administrator { get; set; }
        public List<string> Permissions { get; set; }
    }
}
