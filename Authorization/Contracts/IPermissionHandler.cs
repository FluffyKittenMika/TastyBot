using Authorization.Entities;
using Enums.UserPermissions;

namespace Authorization.Contracts
{
    public interface IPermissionHandler
    {
        bool IsAdministrator(ulong id);
        bool HasPermissions(ulong id, Permissions permission);
    }
}