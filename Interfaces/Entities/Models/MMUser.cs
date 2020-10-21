using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace Interfaces.Entities.Models
{
    public class MMUser
    {

        [BsonId]
        public ulong Id { get; set; }
        public ulong UserId { get; set; }
        public int Wins { get; set; }
    }
}
