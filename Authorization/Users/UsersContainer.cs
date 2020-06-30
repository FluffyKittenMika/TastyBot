using Authorization.Entities;
using Authorization.Contracts;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Authorization.Users
{
    public class UsersContainer : IUsersContainer
    {
        private const string fileLocation = "../../../../Authorization/Users/users.json";

        public IEnumerable<User> GetAll()
        {
            return JsonConvert.DeserializeObject<List<User>>(File.ReadAllText(fileLocation));
        }

        public User ById(ulong id)
        {
            IEnumerable<User> users = GetAll();
            User userOut = null;
            foreach(var user in users)
            {
                if(user.DiscordId == id)
                {
                    userOut = user;
                    break;
                }
            }
            return userOut;
        }

        public void Create(User userCreate)
        {
            throw new NotImplementedException();
        }

        public void Update(User userUpdate)
        {
            throw new NotImplementedException();
        }

        public void Delete(User userDelete)
        {
            throw new NotImplementedException();
        }
    }
}
