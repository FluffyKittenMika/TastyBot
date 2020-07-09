using HeadpatDungeon.Models.Enums;

namespace HeadpatDungeon.Models
{
    /// <summary>
    /// Abstract item class, so we can always override functions, or use the base function of this class. Not using an interface as it does not allow for base.Use();
    /// And we would load items from JSON, so we got a few things to do
    /// </summary>
    public class HpItem
    {

        /// <summary>
        /// Create new item
        /// </summary>
        /// <param name="Type">Item Type</param>
        /// <param name="Name">Optional, Name of the item</param>
        /// <param name="Description">Optional, Description of the item</param>
        public HpItem(ItemDefinition type, string name = "Undefined", string description = "Undefined", int itemlvl = 3, int itemxp = 5, int itemcount = 1)
        {
            Type = type;
            ItemName = name;
            Description = description;
            ItemCount = itemcount;
            ItemLevel = itemlvl;
            ItemXp = itemxp;
        }

        /// <summary>
        /// Constructor, empty, DEFINE IT PROPERLY PLEASE
        /// </summary>
        public HpItem()
        {

        }

        /// <summary>
        /// Keeps track of how many of an item the player has/holds
        /// </summary>
        public int ItemCount { get; set; }

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
        /// Name of the item
        /// </summary>
        public string ItemName { get; set; }


        /// <summary>
        /// The level required to use the item/gather the item
        /// </summary>
        public int ItemLevel { get; set; }

        /// <summary>
        /// This is only used if the item grants any XP on crafting
        /// </summary>
        public int ItemXp { get; set; }

    }
}
