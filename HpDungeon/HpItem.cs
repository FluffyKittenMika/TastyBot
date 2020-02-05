using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace TastyBot.HpDungeon
{


    /// <summary>
    /// This class keeps items in memory
    /// </summary>
    public static class Container
    {
        public static Dictionary<string, HpItem> ItemList = null; //everything list
        public static Dictionary<string, HpItem> OreList = null; //specialised



        /// <summary>
        /// This must be called to populate the global item list.
        /// </summary>
        public static void LoadItems()
        {
            ItemList = new Dictionary<string, HpItem>();
            OreList = new Dictionary<string, HpItem>();


            //i'll just start with ores, and i'll simply add more files to keep it organised
            //and ofc make a proper file function then
            string json = File.ReadAllText(@".\HpDungeon\HpDataFiles\ores.json");
            Dictionary<string, HpItem> temp  = JsonConvert.DeserializeObject<Dictionary<string, HpItem>>(json);
            foreach (var item in temp)
            {
                OreList.Add(item.Key, item.Value);
                ItemList.Add(item.Key, item.Value);

            }

        }

    }


    /// <summary>
    /// Probably will need a better definition system
    /// They're numbered so i don't have to count 
    /// </summary>
    public enum ItemDefinition
    {
        Weapon              = 0 ,
        Consumable          = 1 ,
        Junk                = 2 ,
        CraftingMaterial    = 3 ,
        Armour              = 4 ,
        Helmet              = 5 ,
        Gloves              = 6 ,
        Ring                = 7 ,
        Trinket             = 8 ,
        Belt                = 9 ,
        Wrist               = 10,
        Shoulders           = 11,
        Neck                = 12,
        Eyes                = 13,   //I like glasses ok?
        Head                = 14,
        HeadBand            = 15,   //Banzai!
        Feet                = 16    //Feets are for walking nico
    }


    /// <summary>
    /// Abstract item class, so we can always override functions, or use the base function of this class. Not using an interface as it does not allow for base.Use();
    /// And we would load items from JSON, so we got a few things to do
    /// </summary>
    public class HpItem
    {

        /// <summary>
        /// Create new item
        /// </summary>
        /// <param name="Type">Item Type</param>
        /// <param name="Name">Optional, Name of the item</param>
        /// <param name="Description">Optional, Description of the item</param>
        public HpItem(ItemDefinition type, string name = "Undefined", string description ="Undefined" )
        {
            Type = type;
            ItemName = name;
            Description = description;
        }

        /// <summary>
        /// The type of item
        /// We use an enum here so we know what slot / effect it should iterate through
        /// </summary>
        public ItemDefinition Type { get; set; }

        /// <summary>
        /// Description of the item
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Name of the item
        /// </summary>
        public string ItemName { get; set; }


        /// <summary>
        /// The level required to use the item
        /// </summary>
        public int ItemLevel { get; set; }

        /// <summary>
        /// Use the item, atm does nothing
        /// </summary>
        public void Use() {
            return;
        }

    }
}


