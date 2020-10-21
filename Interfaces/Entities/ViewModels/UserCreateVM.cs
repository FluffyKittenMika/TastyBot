using System;
using System.Collections.Generic;
using System.Text;

namespace Interfaces.Entities.ViewModels
{
    public class UserCreateVM
    {
        public string Name { get; set; }
        public ulong DiscordId { get; set; }
        public bool Administrator { get; set; }
        public List<string> Permissions { get; set; }
    }
}
