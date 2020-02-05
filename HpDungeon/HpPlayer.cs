using System;
using System.Collections.Generic;
using System.Text;

namespace TastyBot.HpDungeon
{
    /// <summary>
    /// This class will mostly just be a Json logic bomb
    /// </summary>
    class HpPlayer : HpCreature
    {

        /// <summary>
        /// Player's inventory.
        /// Basic for now
        /// </summary>
        public List<HpItem> Items { get; set; }

        /// <summary>
        /// The unique id of the player, we get this from discord.
        /// </summary>
        public string ID { get; set; }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="PlayerID">Sets the player ID, based on unique discord ID</param>
        public HpPlayer(string PlayerID)
        {
            ID = PlayerID;
        }

        public override string ToString()
        {
            string s = " ";
            foreach (var item in Items)
            {
                s += item.ItemName + "; ";
            }
            return "This is player " + ID + " Inventory:" + s;
        }

    }
}
