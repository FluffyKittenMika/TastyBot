using System;
using System.Collections.Generic;
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
        public HpItem Craft(Recepie target, out List<HpItem> inventory)
        {
            foreach (var needs in collection)
            {

            }


            inventory = inventory;
        }
    }
}
