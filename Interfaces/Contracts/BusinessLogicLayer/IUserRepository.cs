using Interfaces.Entities.Models;

namespace Interfaces.Contracts.BusinessLogicLayer
{
    public interface IUserRepository
    {
        User ByDiscordId(ulong discordId);
        User ById(ulong id);
        void Create(User userCreate);
        bool Delete(User user);
        bool Update(User user);
    }
}