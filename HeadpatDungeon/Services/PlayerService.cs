using HeadpatDungeon.Models.Entities;
using HeadpatDungeon.Contracts;
using System;

namespace HeadpatDungeon.Services
{
    public class PlayerService : IPlayerService
    {
        public HpPlayer GetPlayer(ulong Id)
        {
            HpPlayer player = null;
            throw new NotImplementedException();
        }

        public void SavePlayer(HpPlayer player)
        {
            throw new NotImplementedException();
        }

        public void SaveAllPlayers()
        {
            throw new NotImplementedException();
        }

        public void SavePlayerDisk(HpPlayer player)
        {
            throw new NotImplementedException();
        }
    }
}
