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
				Console.WriteLine("New player: " + user.Id.ToString());
				p = new HpPlayer(user.Id.ToString())
				{
					ID = user.Id.ToString(),
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
					Container.OreList.TryGetValue(orename, out item);	//Try to get what the player wants
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);                       //truncate mistakes
				}
				if (item != null && item.ItemLevel > p.GetSkillLevel("mining"))	//if it fails, well then we can't go on anyways
						item = null;
																		//we should inform them but we're not for now
			}

			if (item == null)                                           //if it fails, get a random item
			{
																		//Keep getting items untill you get one you can Make/Gather, this is not efficient :)
				item = Container.OreList.ElementAt(_random.Next(Container.OreList.Count)).Value;
				while (item.ItemLevel > p.GetSkillLevel("mining"))
					item = Container.OreList.ElementAt(_random.Next(Container.OreList.Count)).Value;
			}

			p.AddItem(item);
			bool levelup = false;                                       //Prepare to check if they got a lvl
			int currlvl = p.GetSkillLevel("mining");                    //Remember current lvl
			p.AddXP("mining", item.ItemXp);                             //Add the gained xp
			if (currlvl < p.GetSkillLevel("mining"))					//Check if new current lvl is higher than old
				levelup = true;                                         //If yes, then they've gained a lvl

			await SavePlayer(p);                                        //Now we save the player
																		//And compile the resposne
			string response = $"You've gone mining, and got an **{item.ItemName}** \r\n" +
							  $"and gained {item.ItemXp}xp";
			if (levelup)
				response += $"\r\nYou gained a **Mining level!** \r\n Your level is now **{p.GetSkillLevel("mining")}**";

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

			StringBuilder sb = new StringBuilder();

			foreach (var item in p.Items) //TODO: Find a better padding method, discord hates whitespaces, and '\u202F' is not pretty..
				sb.AppendFormat("{0,16} - {1,4} units - ILVL: {2,4}\n", item.Value.ItemName.PadRight(16, '	'), item.Value.ItemCount.ToString().PadRight(4, '	'), item.Value.ItemLevel.ToString().PadRight(4, '	'));

			builder.AddField(x =>
			{
				x.Name = "inv";
				x.Value = sb.ToString();
				x.IsInline = false;
			});
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
				x.Value = skills;
				x.IsInline = false;
			});
			await ReplyAsync("", false, builder.Build());
		}
	}
}
