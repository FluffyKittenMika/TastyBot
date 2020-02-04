using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace TastyBot.HpDungeon
{

    /// <summary>
    /// Probably will need a better definition system
    /// </summary>
    public enum ItemDefinition
    {
        Weapon,
        Consumable,
        Junk,
        Armour,
        Helmet,
        Gloves,
        Ring,
        Trinket,
        Belt,
        Wrist,
        Shoulders,
        Neck,
        Eyes, //I like glasses ok?
        Head,
        HeadBand, //Banzai!
        Feet //Feets are for walking nico
    }


    /// <summary>
    /// Abstract item class, so we can always override functions, or use the base function of this class. Not using an interface as it does not allow for base.Use();
    /// And we would load items from JSON, so we got a few things to do
    /// </summary>
    abstract class HpItemBase
    {
        /// <summary>
        /// The type of item
        /// We use an enum here so we know what slot / effect it should iterate through
        /// </summary>
        public ItemDefinition Type { get; set; }

        /// <summary>
        /// Description of the item
        /// </summary>
        public string Description { get; set; }


        /// <summary>
        /// Use the item, atm does nothing
        /// </summary>
        public void Use() {
            return;
        }

    }
}
