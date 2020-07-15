using Interfaces.Contracts.BusinessLogicLayer;
using Interfaces.Entities.Models;

namespace BusinessLogicLayer.Services
{
    public class UserService : IUserService
    {
        public readonly IUserRepository _repo;

        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }

        public void Create(User userCreate)
        {
            _repo.Create(userCreate);
        }

        public bool Update(User user)
        {
            return _repo.Update(user);
        }

        public bool Delete(User user)
        {
            return _repo.Delete(user);
        }

        public User ById(ulong id)
        {
            return _repo.ById(id);
        }

        public User ByDiscordId(ulong discordId)
        {
            return _repo.ByDiscordId(discordId);
        }
    }
}
