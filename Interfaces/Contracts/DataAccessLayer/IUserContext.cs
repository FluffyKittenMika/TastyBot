using Interfaces.DataTransferObjects;

namespace Interfaces.Contracts.DataAccessLayer
{
    public interface IUserContext
    {
        UserDTO ByDiscordId(ulong discordId);
        UserDTO ById(ulong id);
        void Create(UserDTO userCreate);
        bool Delete(UserDTO userDelete);
        bool Update(UserDTO userUpdate);
    }
}