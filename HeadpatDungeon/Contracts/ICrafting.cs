using HeadpatDungeon.Models.Entities;

namespace HeadpatDungeon.Contracts
{
    public interface ICrafting
    {
        bool Craft(string target, ref HpPlayer player);
    }
}