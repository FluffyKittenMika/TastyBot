using System;
using System.Collections.Generic;
using System.Linq;
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
		public HpDungeonModule(CommandService service, Random random, IConfigurationRoot config)
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
		private async Task<HpPlayer> GetPlayer(IUser user)
		{
			//makes sure there's shit inn there
			if (Container.ItemList == null)
				Container.LoadItems();

			HpPlayer p = null;

			try
			{
				p = (await fileManager.LoadData<HpPlayer>(user.Id.ToString())).FirstOrDefault();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message + " Earan fucked up somewhere :)"); //this means Earan fucked up
			}

			//if it's null, we make a new one, and init it
			if (p == null)
			{
				Console.WriteLine("New player: " + user.Username + "#" + user.Discriminator);
				p = new HpPlayer(user.Username + "#" + user.Discriminator)
				{
					Items = new List<HpItem>(),
					ID = user.Username + "#" + user.Discriminator,
					Skills = new Dictionary<string, int>()
				};
				await SavePlayer(p); //make base save aswell.
			}
			return p;
		}

		private async Task SavePlayer(HpPlayer player)
		{
			//yes i'm this lazy
			List<HpPlayer> p = new List<HpPlayer>
			{
				player
			};
			await fileManager.SaveData(p, player.ID);
		}

		[Command("mine")]
		[Summary("Go gather ores")]
		public async Task Mine([Remainder] string orename = null)
		{
			HpPlayer p = await GetPlayer(Context.User);                       //Get the relevant user context

			HpItem item = null;
			if (!string.IsNullOrEmpty(orename))                         //get specified item
			{
				orename = orename.ToLower();                            //Make it all lowercase for key
				try
				{
					Container.OreList.TryGetValue(orename, out item);  //Try to get what the player wants
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);                        //truncate mistakes
				}
				if (item != null)//if it fails, well then we can't go on anyways
				{
					//check if item is even minable at their level
					if (item.ItemLevel > p.GetSkillLevel("mining"))
					{
						item = null;
					}
				}//we should inform them but we're not for now
			}

			if (item == null)                                           //if it fails, get a random item
			{
				//Keep getting items untill you get one you can Make/Gather, this is not efficient :)
				item = Container.OreList.ElementAt(_random.Next(Container.OreList.Count)).Value;
				while (item.ItemLevel > p.GetSkillLevel("mining"))
				{
					item = Container.OreList.ElementAt(_random.Next(Container.OreList.Count)).Value;
				}
			}

			p.Items.Add(item);
			bool levelup = false;                                       //Prepare to check if they got a lvl
			int currlvl = p.GetSkillLevel("mining");                    //Remember current lvl
			p.AddXP("mining", item.ItemXp);                             //Add the gained xp
			if (currlvl < p.GetSkillLevel("mining"))                    //Check if new current lvl is higher than old
				levelup = true;                                         //If yes, then they've gained a lvl

			await SavePlayer(p);                                              //Now we save the player
																			  //And compile the resposne
			string response = $"You've gone mining, and got an **{item.ItemName}**" +
							  $"\r\n and gained {item.ItemXp}xp";
			if (levelup)
			{
				response += $"\r\nYou gained a **Mining level!** \n Your level is now **{p.GetSkillLevel("mining")}**";
			}

			await ReplyAsync(response);
		}

		[Command("inventory")]
		[Alias("inv")]
		public async Task Inventory()
		{
			HpPlayer p = await GetPlayer(Context.User);

			var builder = new EmbedBuilder()
			{
				Color = new Color(255, 233, 0),
				Title = "Inventory"
			};

			//Compact duplicates
			//Using black LINQ magic
			var q = from x in p.Items
					group x by x.ItemName into g
					let count = g.Count()
					orderby count descending
					select new { Name = g.Key, Count = count, ID = g.First().Description + "\n ItemLevel:" + g.First().ItemLevel };

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



		[Command("skills")]
		[Alias("skill")]
		public async Task Skill()
		{
			HpPlayer p = await GetPlayer(Context.User);

			var builder = new EmbedBuilder()
			{
				Color = new Color(0, 255, 0),
				Title = "Skills"
			};

			var skills = "";
			foreach (var skill in p.Skills)
				skills += $"{skill.Key}: Lvl {p.XPToLevel(skill.Value)} Exp: {skill.Value} / {p.LevelToXP(p.XPToLevel(skill.Value) + 1)}\n";

			builder.AddField(x =>
			{
				x.Name = "Skills";
				x.Value = skills;
				x.IsInline = false;
			});
			await ReplyAsync("", false, builder.Build());
		}
	}
}
