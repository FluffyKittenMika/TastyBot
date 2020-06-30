using System;
using System.Collections.Generic;
using System.Text;

namespace Authorization.Entities
{
    public class User
    {
        public ulong DiscordId { get; set; }
        public bool Administrator { get; set; }
    }
}
