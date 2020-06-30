using HeadpatDungeon.Models;
using System.Collections.Generic;

namespace HeadpatDungeon.Contracts
{
    public interface IItemContainer
    {
        Dictionary<string, HpItem> GetItemList();
        Dictionary<string, HpItem> GetOreList();
        void LoadItems();
    }
}