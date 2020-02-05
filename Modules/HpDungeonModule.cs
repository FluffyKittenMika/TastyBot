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
        private readonly Random _random;
        /// <summary>
        /// Constructor, this one is called automagically through reflection
        /// </summary>
        /// <param name="service">Relevant service</param>
        /// <param name="config">Relevant config</param>
        public HpDungeonModule(CommandService service,Random random, IConfigurationRoot config)
        {
            _service = service;
            _config = config;
            _random = random;
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
                p = fileManager.LoadData<HpPlayer>(user.Username + "#"+user.Discriminator).Result.FirstOrDefault();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + " Earan fucked up somewhere :)"); //this means Earan fucked up
            }

            //if it's null, we make a new one, and init it
            if (p == null)
            {
                Console.WriteLine("New player: " + user.Username + "#" + user.Discriminator); //this means Earan fucked up
                p = new HpPlayer(user.Username + "#" + user.Discriminator)
                {
                    Items = new List<HpItem>()
                };
                
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

            //take 1 random item from the ore list, we'll do some level checking at a later date, this works for now.
            HpItem item = Container.OreList.Take(1).FirstOrDefault().Value;
            p.Items.Add(item);

            //now we gotta save it once we're done editing it.
            SavePlayer(p);
            await ReplyAsync("You've gone mining, and got a " + item.ItemName);
        }

        [Command("inventory")]
        [Alias("inv")]
        public async Task Inventory(IUser user = null)
        {
            user ??= Context.User;
            HpPlayer p = GetPlayer(user);

            var builder = new EmbedBuilder()
            {
                Color = new Color(255, 233, 0),
                Description = "Your inventory mi lordship"
            };

            //Compact duplicates
            var q = from x in p.Items
                    group x by x.ItemName into g
                    let count = g.Count()
                    orderby count descending
                    select new { Name = g.Key, Count = count, ID = g.First().Description};

            foreach (var item in q)
            {
                builder.AddField(x =>
                {
                    x.Name = item.Name + ": " + item.Count;
                    x.Value = item.ID;
                    x.IsInline = false;
                }); 
            }
            await ReplyAsync("", false, builder.Build());

        }

    }
}
