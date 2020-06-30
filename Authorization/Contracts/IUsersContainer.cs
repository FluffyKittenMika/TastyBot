using Authorization.Entities;
using System.Collections.Generic;

namespace Authorization.Contracts
{
    public interface IUsersContainer
    {
        User ById(ulong id);
        void Create(User userCreate);
        void Delete(User userDelete);
        IEnumerable<User> GetAll();
        void Update(User userUpdate);
    }
}