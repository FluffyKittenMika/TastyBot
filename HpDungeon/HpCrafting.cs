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
        public Dictionary<string, int> Requirements { get; set; }
       
        /// <summary>
        /// Output
        /// </summary>
        public HpItem Result { get; set; }

        /// <summary>
        /// The relevant skill
        /// </summary>
        public string Skill { get; set; }
    }


    /// <summary>
    /// Crafting class. 
    /// </summary>
    public class HpCrafting
    {
        /// <summary>
        /// Makes an item
        /// </summary>  
        /// <param name="target">What we want to make</param>
        /// <param name="player">player we'll query the inventory from</param>
        /// <returns>True if it was a success, otherwise false.</returns>
        public bool Craft(string target, ref HpPlayer player)
        {
            bool CanCraft = true;

            if (Container.Recepies.ContainsKey(target))
            {
                Recepie recepie = Container.Recepies[target];
                foreach (var targetItemReq in recepie.Requirements) //for every requirement, we check if they have enough of the required item.
                {
                    if (player.Items.ContainsKey(targetItemReq.Key)) //check if they got the item
                    {
                        if (targetItemReq.Value > player.Items[targetItemReq.Key].ItemCount) //check the amount they got, and if they don't have enough, we break and end it all
                        {
                            CanCraft = false;
                            break; //stop looping, and go on as we can't craft this item.
                        }
                    }
                    else
                    {
                        //they don't have the item. So we break
                        CanCraft = false;
                        break;
                    }
                }

                //we only go inn here if we can craft the item, and they passed the above requirements
                if (CanCraft && recepie.Result.ItemLevel <= player.GetSkillLevel(recepie.Skill))
                {
                    //remove the stuff the player has
                    foreach (var targetItemReq in recepie.Requirements)
                        player.RemoveItem(targetItemReq.Key);

                    //Rewards
                    player.AddXP(recepie.Skill, recepie.Result.ItemXp);
                    player.AddItem(recepie.Result);
                }
            }
            return CanCraft;
        }
    }
}
