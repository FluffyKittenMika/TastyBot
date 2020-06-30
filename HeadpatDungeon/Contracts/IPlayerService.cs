using HeadpatDungeon.Models.Entities;

namespace HeadpatDungeon.Contracts
{
    public interface IPlayerService
    {
        HpPlayer GetPlayer(ulong Id);
        void SaveAllPlayers();
        void SavePlayer(HpPlayer player);
        void SavePlayerDisk(HpPlayer player);
    }
}