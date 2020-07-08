using HeadpatDungeon.Models;
using HeadpatDungeon.Models.Entities;
using HeadpatDungeon.Contracts;

namespace HeadpatDungeon.Strategies
{
    /// <summary>
    /// Crafting class. 
    /// </summary>
    public class Crafting
    {
        private readonly IRecipeContainer _recipeContainer;

        public Crafting(IRecipeContainer recipeContainer)
        {
            _recipeContainer = recipeContainer;
        }


        public IRecipeContainer GetRecepies()
        {
            return _recipeContainer;
        }

        /// <summary>
        /// Makes an item
        /// </summary>  
        /// <param name="target">What we want to make</param>
        /// <param name="player">player we'll query the inventory from</param>
        /// <returns>True if it was a success, otherwise false.</returns>
        public bool Craft(string target, ref HpPlayer player)
        {
            bool CanCraft = true;

            if (_recipeContainer.GetRecipesList().ContainsKey(target))
            {
                Recipe recepie = _recipeContainer.GetRecipesList()[target];
                foreach (var targetItemReq in recepie.Requirements) //for every requirement, we check if they have enough of the required item.
                {
                    if (player.Inventory.ContainsKey(targetItemReq.Key)) //check if they got the item
                    {
                        if (targetItemReq.Value > player.Inventory[targetItemReq.Key].ItemCount) //check the amount they got, and if they don't have enough, we break and end it all
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
                    player.AddXPToSkill(recepie.Skill, recepie.Result.ItemXp);
                    player.AddItem(recepie.Result);
                    return CanCraft;
                }
            }
            return false;
        }
    }
}
