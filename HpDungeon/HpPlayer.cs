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
        /// Simple skill system, where we just define
        /// </summary>
        public Dictionary<string,int> Skills { get; set; }


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

        /// <summary>
        /// Override ToString
        /// </summary>
        /// <returns>Generic description</returns>
        public override string ToString()
        {
            string s = " ";
            foreach (var item in Items)
                s += item.ItemName + "; ";

            return "This is player " + ID + " Inventory:" + s + " Base: " + base.ToString();
        }


        /// <summary>
        /// Returns the current level, calculated from XP
        /// </summary>
        /// <param name="skill">The skill name</param>
        public int GetSkillLevel(string skill)
        {
            //no Skills defined
            if (Skills == null)
                return 0;

            if (Skills.ContainsKey(skill.ToLower()))
            {
                int xp = Skills[skill.ToLower()];

                return XPToLevel(xp);
            }

            return 0;
        }

        /// <summary>
        /// Add xp to a skill
        /// </summary>
        /// <param name="skill">The skill to add the XP to</param>
        /// <param name="xp">How much XP you want to add</param>
        public void AddXP(string skill, int xp) 
        {
            skill = skill.ToLower();

            //make sure there's a skill storage
            if (Skills == null)
                Skills = new Dictionary<string, int>();

            //add xp to it, else add it with that amount of xp
            if (Skills.ContainsKey(skill))
                Skills[skill] += xp;
            else
                Skills.Add(skill, xp);

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
