using System;
using System.Collections.Generic;
using System.Linq;

namespace TastyBot.HpDungeon
{

    //TODO: Make entire player Class async?
    /// <summary>
    /// This class will mostly just be a Json logic bomb
    /// </summary>
    public class HpPlayer : HpCreature
    {


        /// <summary>
        /// Simple skill system, where we just define
        /// </summary>
        public Dictionary<string, int> Skills { get; set; }


        /// <summary>
        /// Player's inventory.
        /// </summary>
        public Dictionary<string, HpItem> Items { get; set; }


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

        /// <summary>
        /// Override ToString
        /// </summary>
        /// <returns>Generic description</returns>
        public override string ToString()
        {
            string s = " ";
            foreach (var item in Items)
                s += item.Value.ItemName + "; ";

            return "This is player " + ID + " Inventory:" + s + " Base: " + base.ToString();
        }


        /// <summary>
        /// Returns the current level, calculated from XP
        /// This is safe to call, as the minimum it returns is 3, never null.
        /// </summary>
        /// <param name="skill">The skill name</param>
        public int GetSkillLevel(string skill)
        {
            //no Skills defined
            if (Skills == null)
                return 3; //minimum lvl 3

            if (Skills.ContainsKey(skill.ToLower()))
            {
                int xp = Skills[skill.ToLower()];

                if (XPToLevel(xp) < 3)
                    return 3; //minimum lvl
                return XPToLevel(xp);
            }

            return 3; //minimum lvl 3
        }





        /// <summary>
        /// Use this to add items to a players inventory
        /// </summary>
        /// <param name="item">The item to add</param>
        public void AddItem(HpItem item)
        {
            //Make sure there's an inventory
            if (Items == null)
                Items = new Dictionary<string, HpItem>(StringComparer.OrdinalIgnoreCase);

            if (Items.ContainsKey(item.ItemName))
                Items[item.ItemName].ItemCount++;
            else
            {
                //We do not want it to say, you got 0 apples. when we do have 1.
                if (item.ItemCount == 0)
                    item.ItemCount = 1;
                Items.Add(item.ItemName, item);
            }
        }


        /// <summary>
        /// Use this to substract 1 from any item in an inventory
        /// </summary>
        /// <param name="item">The name of the item you want to remove</param>
        public void RemoveItem(string item)
        {
            if (Items == null) //HALT EARAN, HE'S ESCAPING
                return;

            if (Items.ContainsKey(item))
            {
                Items[item].ItemCount--;
                if (Items[item].ItemCount <= 0) //We remove this item
                    Items.Remove(item);
            }
            //Nothing to remove if not.
        }

        /// <summary>
        /// Add xp to a skill
        /// </summary>
        /// <param name="skill">The skill to add the XP to</param>
        /// <param name="xp">How much XP you want to add</param>
        /// <returns>True if gained level, false otherwise</returns>
        public bool AddXP(string skill, int xp)
        {
            skill = skill.ToLower();
            bool levelup = false;

            //make sure there's a skill storage
            if (Skills == null)
                Skills = new Dictionary<string, int>();

            //add xp to it, else add it with that amount of xp
            if (Skills.ContainsKey(skill))
            {
                int currlvl = GetSkillLevel(skill);     
                Skills[skill] += xp;
                if (currlvl < GetSkillLevel(skill))
                    levelup = true;
            }
            else
                Skills.Add(skill, xp);

            return levelup;
        }

        public int Equate(int xp)
        {
            return (int)Math.Floor(
                xp + 300 * Math.Pow(2, xp / 7));
        }

        public int LevelToXP(int level)
        {
            double xp = 0;

            for (int i = 1; i < level; i++)
                xp += Equate(i);

            return (int)Math.Floor(xp / 4);
        }

        public int XPToLevel(int xp)
        {
            int level = 1;

            while (LevelToXP(level) < xp)
                level++;

            return level;
        }


    }
}
