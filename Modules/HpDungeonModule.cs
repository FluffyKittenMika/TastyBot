using FileManager.Contracts;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;

using TastyBot.Utility;
using Authorization.Contracts;
using HeadpatDungeon.Contracts;
using HeadpatDungeon.Models.Entities;

namespace TastyBot.HpDungeon
{
    /*
    [Name("HpDungeon")]
	public class HpDungeonModule : ModuleBase<SocketCommandContext>
	{
		#region Definitions
		private readonly IHpDModule _module;
		private readonly IFileManager _fileManager;
		private readonly IPermissionHandler _permissionHandler;
		private readonly Config _config;
		private readonly Random _random = new Random();

		//Static retains memory. 
		private static Dictionary<string,HpPlayer> PlayerRamMemory = new Dictionary<string, HpPlayer>();
		private static int savecounter = 0;

        #endregion

        #region Functions
        /// <summary>
        /// Constructor, this one is called automagically through reflection
        /// </summary>
        /// <param name="service">Relevant service</param>
        /// <param name="config">Relevant config</param>
        public HpDungeonModule(IHpDModule module, IFileManager fileManager, IPermissionHandler permissionHandler, Config config)
		{
			_module = module;
			_fileManager = fileManager;
			_fileManager.Init();
			_permissionHandler = permissionHandler;

			_config = config;
		}

		///// <summary>
		///// This is run every time we get a command, it's how we fetch the player file, and actually do anything, it also checks if the item pool exists
		///// </summary>
		///// <returns>A new player, or auto loads a player</returns>
		//private async Task<HpPlayer> GetPlayer(IUser user)
		//{
		//	//TODO: Find a better way that uses time or something. Every 5'th message is kinda not that good
		//	savecounter++;
		//	if (savecounter > Convert.ToInt32(_config.Savecounter))
		//	{
		//		SaveAllPlayers();
		//		savecounter = 0;
		//	}
		//	HpPlayer p = null;

		//	if (PlayerRamMemory.ContainsKey(user.Id.ToString())) //if it already is loaded
		//	{
		//		p = PlayerRamMemory[user.Id.ToString()] ;
		//	}
		//	else //fetch from slow storage
		//	{
		//		try
		//		{
		//			p = (await _fileManager.LoadData<HpPlayer>(user.Id.ToString())).FirstOrDefault();
		//		}
		//		catch (Exception e)
		//		{
		//			Console.WriteLine(e.Message + " Earan fucked up somewhere :)"); //this means Earan fucked up
		//		}

		//		//if it's null, we make a new one, and init it
		//		if (p == null)
		//		{
		//			Console.WriteLine("New player: " + user.Id.ToString());
		//			p = new HpPlayer(user.Id.ToString())
		//			{
		//				ID = user.Id.ToString(),
		//				Skills = new Dictionary<string, int>()
		//			};
		//			SavePlayerDisk(p); //make base save aswell.
		//		}
		//		//add to ram
		//		PlayerRamMemory.Add(user.Id.ToString(), p);
		//	}
		
		//	return p;
		//}


		////TODO Move this to a different class
		///// <summary>
		///// Stores the player in the ram module
		///// </summary>
		///// <param name="player">Target player to store</param>
		///// <returns></returns>
		//private void SavePlayer(HpPlayer player)
		//{
		//	if (PlayerRamMemory.ContainsKey(player.ID))
		//		PlayerRamMemory[player.ID] = player;
		//	else
		//		PlayerRamMemory.Add(player.ID, player);
		//}

		///// <summary>
		///// Saves all players from ram to Disk
		///// </summary>
		///// <returns>Fuck all</returns>
		//private void SaveAllPlayers()
		//{
		//	foreach (var p in PlayerRamMemory)
		//	{
		//		SavePlayerDisk(p.Value);
		//	}
		//	Console.WriteLine("All players saved");
		//}

		///// <summary>
		///// Stores a single player to disk from ram
		///// </summary>
		///// <param name="player">Target player</param>
		///// <returns>Nothing at all</returns>
		//private void SavePlayerDisk(HpPlayer player)
		//{
		//	//yes i'm this lazy
		//	List<HpPlayer> p = new List<HpPlayer>
		//	{
		//		player
		//	};
		//	_fileManager.SaveData(p, player.ID);
		//}


		#endregion

		[Command("hpdsave")]
		public async Task HpdSavePlayers()
		{
			if (_permissionHandler.IsAdministrator(Context.User.Id))
            {
				await ReplyAsync(_module.HPDSavePlayers());
				return;
			}
			await ReplyAsync("ask mik to run it. as this is global save.");
		}

		[Command("agility")]
		[Summary("Train agility, the skill does nothing atm")]
		public async Task Agility()
		{
			await ReplyAsync(_module.Action("agility", Context.User.Id));
		}


		[Command("mine")]
		[Summary("Go gather ores")]
		public async Task Mine([Remainder] string orename = null)
		{
			await ReplyAsync(_module.Action("mine", Context.User.Id));
		}

		[Command("inventory")]
		[Alias("inv")]
		[Summary("Get your inventory")]
		public async Task Inventory()
		{
			HpPlayer p = await GetPlayer(Context.User);

			var builder = new EmbedBuilder()
			{
				Color = new Color(255, 233, 0),
				Title = "Inventory"
			};

			StringBuilder sb = new StringBuilder();
			sb.Append("```");

			foreach (var item in p.Items) //TODO: Find a better padding method, discord hates whitespaces, and '\u202F' is not pretty..
				sb.AppendFormat("{0,16} - {1,4} units - ILVL: {2,4}\n", item.Value.ItemName.PadRight(16, '	'), item.Value.ItemCount.ToString().PadRight(4, '	'), item.Value.ItemLevel.ToString().PadRight(4, '	'));

			sb.Append("```");
			builder.AddField(x =>
			{
				x.Name = "inv";
				x.Value = sb.ToString();
				x.IsInline = false;
			});
			await ReplyAsync("", false, builder.Build());
		}

		[Command("Craft")]
		[Summary("Craft an object")]
		public async Task Craft([Remainder]string craft)
		{
			//check if whatever the fuck they typed, is a key
			if (Container.Recepies.ContainsKey(craft.ToLower()))
			{
				var i = Container.Recepies[craft.ToLower()];
				HpPlayer p = await GetPlayer(Context.User);

				bool levelup = false;                                      //Prepare to check if they got a lvl
				int currlvl = p.GetSkillLevel(i.Skill);						//Remember current lvl
				var crafted = crafter.Craft(craft.ToLower(), ref p);
				if (currlvl < p.GetSkillLevel(i.Skill))                    //Check if new current lvl is higher than old
					levelup = true;                                        //If yes, then they've gained a lvl


				//And compile the resposne
				StringBuilder response = new StringBuilder();
				if (crafted) //if it did craft the item, and it's all good
				{
					response.Append($"You made a {craft} and gained {i.Result.ItemXp} {i.Skill} XP");
					if (levelup)
						response.Append($"\nYou gained a **{i.Skill} LEVEL!**");
					SavePlayer(p);
					await ReplyAsync(response.ToString());
				}
				else if (!crafted && p.GetSkillLevel(i.Skill) <= i.Result.ItemLevel)
				{
					response.Append($"Insufficient {i.Skill} Level, Requires {i.Skill} lvl: {i.Result.ItemLevel}");
					await ReplyAsync(response.ToString());
				}
				else
				{
					response.Append($"Insufficient materials");
					await ReplyAsync(response.ToString());
				}

			}
		}


		[Command("skills")]
		[Alias("skill")]
		public async Task Skill(IUser otheruser = null)
		{
			HpPlayer p = await GetPlayer(Context.User);
			if (otheruser != null)
				p = await GetPlayer(otheruser);

			var builder = new EmbedBuilder()
			{
				Color = new Color(0, 255, 0),
				Title = "Skills"
			};

			var skills = "";
			foreach (var skill in p.Skills)
				skills += $"{skill.Key}: Lvl {p.GetSkillLevel(skill.Key)} Exp: {skill.Value} / {p.LevelToXP(p.GetSkillLevel(skill.Key) + 1)}\n";

			builder.AddField(x =>
			{
				x.Name = "skills";
				x.Value = skills;
				x.IsInline = false;
			});
			await ReplyAsync("", false, builder.Build());
		}*/
}