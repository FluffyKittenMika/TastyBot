using HeadpatDungeon.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using HeadpatDungeon.Contracts;

namespace HeadpatDungeon.Containers
{
    /// <summary>
    /// This class keeps items in memory
    /// </summary>
    public class ItemContainer : IItemContainer
    {
        //Items
        private Dictionary<string, HpItem> ItemList = null; //everything list
        private Dictionary<string, HpItem> OreList = null; //specialised

        public Dictionary<string, HpItem> GetItemList()
        {
            return ItemList;
        }

        public Dictionary<string, HpItem> GetOreList()
        {
            return OreList;
        }

        /// <summary>
        /// This must be called to populate the global item list.
        /// </summary>
        public void LoadItems()
        {
            //Case insensetive dicts. For my sanity.
            ItemList = new Dictionary<string, HpItem>(StringComparer.OrdinalIgnoreCase);
            OreList = new Dictionary<string, HpItem>(StringComparer.OrdinalIgnoreCase);

            LoadItemList(@".\HpDataFiles\ores.json", ref OreList);
        }

        private void LoadItemList(string path, ref Dictionary<string, HpItem> dict)
        {
            string json = File.ReadAllText(path);
            dict = JsonConvert.DeserializeObject<Dictionary<string, HpItem>>(json);
            foreach (var item in dict)
                ItemList.Add(item.Key, item.Value);
        }
    }
}
