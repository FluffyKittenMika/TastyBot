using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TastyBot.HpDungeon
{



    public class Recepie
    {
        /// <summary>
        /// The Requirements for the result
        /// 
        /// </summary>
        public Dictionary<HpItem, int> Requirements { get; set; }
       
        /// <summary>
        /// Output
        /// </summary>
        public HpItem Result { get; set; }
    }


    class HpCrafting
    {
        /// <summary>
        /// Makes an item
        /// </summary>  
        /// <param name="target">What we want to make</param>
        /// <param name="inventory">Inventory we'll query items from</param>
        /// <returns></returns>
        public List<HpItem> Craft(Recepie target, List<HpItem> inventory)
        {
            bool canCraftItem = false;
          
            foreach (var needs in target.Requirements)
            {
                try
                {
                    //Count the inventory, for each item, that has the needs object in it as a key. 
                    //That gives us the total amount of X item in Inventory of the recepie, and they all have to match up
                    int count = inventory.Count(x => x.Equals(needs.Key));
                    if (count >= needs.Value) // check if the player has enough of said item
                        canCraftItem = true;
                }
                catch (Exception)
                {
                    //can't make the item if it fails.
                    return inventory;
                }
            }


            //if it can craft it after checking all the stuff, it's time we remove that from the player, and give the result
            if (canCraftItem)
            {
                foreach (var substractableitem in target.Requirements)
                {
                    for (int i = 0; i < substractableitem.Value; i++) //we have X amount of those that needs to be removed
                    {
                        inventory.Remove(substractableitem.Key); //remove one of the item
                    }
                }
                //add the result
                inventory.Add(target.Result);
            }
            return inventory;
        }
    }
}
