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

        /// <summary>
        /// The relevant skill
        /// </summary>
        public string Skill { get; set; }

        /// <summary>
        /// The amount of XP rewarded for crafting said item
        /// </summary>
        public int ResultXP { get; set; }
    }


    class HpCrafting
    {
        /// <summary>
        /// Makes an item
        /// </summary>  
        /// <param name="target">What we want to make</param>
        /// <param name="player">player we'll query the inventory from</param>
        /// <returns></returns>
        public void Craft(Recepie target, HpPlayer player)
        {
            //TODO: Write the crafting system.

            bool CanCraft = true;
            foreach (var targetItemReq in target.Requirements) //for every requirement, we check if they have enough of the required item.
            {
                if (player.Items.ContainsKey(targetItemReq.Key.ItemName)) //check if they got the item
                {
                    if (targetItemReq.Value > player.Items[targetItemReq.Key.ItemName].ItemCount) //check the amount they got, and if they don't have enough, we break and end it all
                    {
                        CanCraft = false;
                        break; //stop looping, and go on
                    }
                }
            }

            //we only go inn here if we can craft the item, and they passed the above requirements
            if (CanCraft)
            {
                //remove the stuff the player has
                foreach (var targetItemReq in target.Requirements)
                {
                    player.RemoveItem(targetItemReq.Key);
                }

                player.AddXP(target.Skill, target.ResultXP);
                player.AddItem(target.Result);
            }
        }
    }
}
