using Interfaces.Entities.Models;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace Interfaces.DataTransferObjects
{
    /*
         * Id       Name        DiscordId       AmountOfWins
         * Id       UserId      Wins
        */
    public class MMUserDTO
    {
        public MMUserDTO()
        {

        }
        public MMUserDTO(ulong userId, int wins)
        {
            UserId = userId;
            Wins = wins;
        }
        public MMUserDTO(ulong id, ulong userId, int wins)
        {
            Id = id;
            UserId = userId;
            Wins = wins;
        }

        [BsonId]
        public ulong Id { get; set; }
        public ulong UserId { get; set; }
        public int Wins { get; set; }
    }

}
