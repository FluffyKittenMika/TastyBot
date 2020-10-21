using Interfaces.Contracts.DataAccessLayer;
using Interfaces.Contracts.BusinessLogicLayer;
using Interfaces.DataTransferObjects;
using Interfaces.Entities.Models;

using Enums.UserPermissions;

using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer.Repositories
{
    public class UserRepository : IUserRepository
    {
        public readonly IUserContext _context;

        public UserRepository(IUserContext context)
        {
            _context = context;
        }

        public void Create(User userCreate)
        {
            UserDTO userDTO = UserCreateDTOFromUserCreate(userCreate);
            _context.Create(userDTO);
        }

        public bool Update(User user)
        {
            UserDTO userDTO = UserDTOFromUser(user);
            return _context.Update(userDTO);
        }

        public bool Delete(User user)
        {
            UserDTO userDTO = UserDTOFromUser(user);
            return _context.Delete(userDTO);
        }

        public User ById(ulong id)
        {
            return UserFromUserDTO(_context.ById(id));
        }

        public User ByDiscordId(ulong discordId)
        {
            return UserFromUserDTO(_context.ByDiscordId(discordId));
        }

        private User UserFromUserDTO(UserDTO userDTO)
        {
            if (userDTO == null) return null;
            List<Permissions> permissions = userDTO.Permissions.Select(x => Enum.Parse<Permissions>(x)).ToList();
            User user = new User
            {
                Id = userDTO.Id,
                Name = userDTO.Name,
                DiscordId = userDTO.DiscordId,
                Administrator = userDTO.Administrator,
                Permissions = permissions
            };
            return user;
        }

        private UserDTO UserDTOFromUser(User user)
        {
            if (user == null) return null;
            List<string> permissions = user.Permissions.Select(x => x.ToString()).ToList();
            return new UserDTO(
                user.Id,
                user.Name,
                user.DiscordId,
                user.Administrator,
                permissions);
        }

        private UserDTO UserCreateDTOFromUserCreate(User userCreate)
        {
            if (userCreate == null) return null;
            List<string> permissions = userCreate.Permissions.Select(x => x.ToString()).ToList();
            return new UserDTO(
                userCreate.Name,
                userCreate.DiscordId,
                userCreate.Administrator,
                permissions);
        }
    }
}
