using Interfaces.Contracts.Database;
using Interfaces.Contracts.DataAccessLayer;
using Interfaces.DataTransferObjects;
using LiteDB;

namespace DataAccessLayer.Context
{
    public class UserContext : IUserContext
    {
        private readonly ILiteCollection<UserDTO> _column;
        private readonly ILiteDB _liteDB;
        

        public UserContext(ILiteDB DB)
        {
            _column = DB.GetColumnByName<UserDTO>("users");
            _liteDB = DB;
        }

        public void Create(UserDTO userCreate)
        {
            
            _column.Insert(userCreate);
        }

        public bool Update(UserDTO userUpdate)
        {

            return _column.Update(userUpdate);
        }

        public bool Delete(UserDTO userDelete)
        {

            return _column.Delete(userDelete.Id);
        }

        public UserDTO ById(ulong id)
        {

            return _column.FindById(id);
        }

        public UserDTO ByDiscordId(ulong discordId)
        {

            return _column.FindOne(x => x.DiscordId == discordId);
        }
    }
}
