using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Microsoft.Extensions.Configuration;
using TastyBot.Data;

namespace TastyBot.HpDungeon
{


    [Name("HpDungeon")]
    public class HpDungeonModule : ModuleBase<SocketCommandContext>
    {
        private readonly CommandService _service;
        private readonly IConfigurationRoot _config;
        private readonly FileManager fileManager;
        /// <summary>
        /// Constructor, this one is called automagically through reflection
        /// </summary>
        /// <param name="service">Relevant service</param>
        /// <param name="config">Relevant config</param>
        public HpDungeonModule(CommandService service, IConfigurationRoot config)
        {
            _service = service;
            _config = config;
            fileManager = new FileManager();
            fileManager.Init();
        }




        /// <summary>
        /// This is run every time we get a command, it's how we fetch the player file, and actually do anything, it also checks if the item pool exists
        /// </summary>
        /// <returns>A new player, or auto loads a player</returns>
        private HpPlayer GetPlayer(IUser user) 
        {
            //makes sure there's shit inn there
            if (Container.ItemList == null)
                Container.LoadItems();

            HpPlayer p = null;

            try
            {
                p = fileManager.LoadData<HpPlayer>(user.Username).Result.FirstOrDefault();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + " Earan fucked up somewhere :)"); //this means Earan fucked up
            }

            //if it's null, we make a new one, and init it
            if (p == null)
            {
                p = new HpPlayer(user.Username);
                p.Items = new List<HpItem>();
            }

            return p;
        }

        private void SavePlayer(HpPlayer player)
        {
            //yes i'm this lazy
            List<HpPlayer> p = new List<HpPlayer>
            {
                player
            };
            fileManager.SaveData(p, player.ID);
        }

        [Command("mine")]
        [Summary("Go gather ores")]
        public async Task Mine(IUser user = null, params string[] args)
        {
            user ??= Context.User;
            HpPlayer p = GetPlayer(user);

            p.Items.Add(new HpItem(ItemDefinition.CraftingMaterial,"Test ore"));




            //now we gotta save it once we're done editing it.
            SavePlayer(p);
            await ReplyAsync("You've gone mining, and probably got something, this is still debug :)" + p.ToString());
            //Get shit from an ore list, or just make ores? Hmmm.

            //load player, if one exist, if not, create object.

        }

    }
}
