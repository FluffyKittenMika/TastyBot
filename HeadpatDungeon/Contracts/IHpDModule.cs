using Discord;

namespace HeadpatDungeon.Contracts
{
    public interface IHpDModule
    {
        string Action(string action, ulong id);
        string Craft(string craft);
        string HPDSavePlayers();
        Embed Inventory();
        Embed Skills(IUser user);
    }
}