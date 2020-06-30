using HeadpatDungeon.Models.Entities;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using HeadpatDungeon.Contracts;
using System;

namespace HeadpatDungeon.Containers
{
    public class EntityContainer : IEntityContainer
    {
        private Dictionary<string, DefaultCreature> EntityList = null; // Entity dictonary

        public Dictionary<string, DefaultCreature> GetEntityList()
        {
            return EntityList;
        }

        public void LoadEntities()
        {
            //Case insensetive dicts. For my sanity.
            EntityList = new Dictionary<string, DefaultCreature>(StringComparer.OrdinalIgnoreCase);

            LoadEntityList(@".\HpDataFiles\ores.json", ref EntityList);
        }

        public DefaultCreature GetDefaultPlayer()
        {
            EntityList.TryGetValue("player", out DefaultCreature player);
            return player;
        }

        public DefaultCreature GetDefaultZombie()
        {
            EntityList.TryGetValue("zombie", out DefaultCreature zombie);
            return zombie;
        }

        private void LoadEntityList(string path, ref Dictionary<string, DefaultCreature> dict)
        {
            string json = File.ReadAllText(path);
            dict = JsonConvert.DeserializeObject<Dictionary<string, DefaultCreature>>(json);
        }
    }
}
