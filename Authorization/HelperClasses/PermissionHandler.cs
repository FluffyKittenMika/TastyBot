using Authorization.Entities;
using Authorization.Contracts;
using Enums.UserPermissions;

namespace Authorization.HelperClasses
{
    public class PermissionHandler : IPermissionHandler
    {
        private readonly IUsersContainer _usersContainer;

        public PermissionHandler(IUsersContainer usersContainer)
        {
            _usersContainer = usersContainer;
        }

        public bool IsAdministrator(ulong id)
        {
            User user = _usersContainer.ById(id);
            if(user == null)
            {
                return false;
            }
            return user.Administrator;
        }

        public bool HasPermissions(ulong id, Permissions permission)
        {
            User user = _usersContainer.ById(id);
            return user.Permissions.Contains(permission);
        }
    }
}
