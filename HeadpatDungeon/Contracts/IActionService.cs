using HeadpatDungeon.Models.Entities;

namespace HeadpatDungeon.Contracts
{
    public interface IActionService
    {
        string ExecuteAction(string action, HpPlayer player, string actionObject = null);
    }
}