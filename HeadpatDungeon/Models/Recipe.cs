using System.Collections.Generic;

namespace HeadpatDungeon.Models
{
    public class Recipe
    {
        /// <summary>
        /// The Requirements for the result
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
}
