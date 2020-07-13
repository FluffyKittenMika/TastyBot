using Enums.UserPermissions;
using Interfaces.Entities.Models;
using Interfaces.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Converters
{
    public static class Converter
    {
        public static UserVM UserVMFromUser(User user)
        {
            return new UserVM(user);
        }

        public static string PermissionStringFromPermissionsList(List<Permissions> permissions)
        {
            return string.Join(", ", permissions.Select(x => x.ToString()).ToList());
        }
    }
}
