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
        Dictionary<HpItem, int> Requirements { get; set; }
       
        /// <summary>
        /// Output
        /// </summary>
        HpItem Result { get; set; }
    }


    class HpCrafting
    {

    }
}
