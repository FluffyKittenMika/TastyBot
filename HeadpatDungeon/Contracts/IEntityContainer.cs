using HeadpatDungeon.Models.Entities;
using System.Collections.Generic;

namespace HeadpatDungeon.Contracts
{
    public interface IEntityContainer
    {
        Dictionary<string, DefaultCreature> GetEntityList();
        public void LoadEntities();
        DefaultCreature GetDefaultPlayer();
        DefaultCreature GetDefaultZombie();
    }
}