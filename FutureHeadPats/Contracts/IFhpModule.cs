using Discord;

namespace FutureHeadPats.Contracts
{
    public interface IFhpModule
    {
        void Give(IUser receiver, long amount);
        string Delete(IUser deleteUser);
        string Save();
        string Pat(IUser sender, IUser receiver, int amount = 1);
        Embed Wallet(IUser user);
        Embed Leaderboard();
        string Explain();
    }
}