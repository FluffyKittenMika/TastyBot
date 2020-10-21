using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;

using DiscordUI.Utility;
using HeadpatDungeon.Contracts;
using HeadpatDungeon.Models.Entities;
using Utilities.FileManager;
using Utilities.LoggingService;
using Interfaces.Contracts.BusinessLogicLayer;
using HeadpatDungeon.Strategies;

namespace DiscordUI.HpDungeon
{

	[Name("HpDungeon")]
	public class HpDungeonModule : ModuleBase<SocketCommandContext>
	{
		#region Definitions
		private readonly IHpDModule _module;
		private readonly IUserRepository _user;
		private readonly ICrafting _crafting;

		//Static retains memory. 
		private static readonly Dictionary<ulong, HpPlayer> PlayerRamMemory = new Dictionary<ulong, HpPlayer>();
		private static int savecounter = 0;

		#endregion

		#region Functions
		/// <summary>
		/// Constructor, this one is called automagically through reflection
		/// </summary>
		/// <param name="service">Relevant service</param>
		/// <param name="config">Relevant config</param>
		public HpDungeonModule(IHpDModule module, IUserRepository user, ICrafting crafting)
		{
			_module = module;
			_user = user;
			_crafting = crafting;
		}

		/// <summary>
		/// This is run every time we get a command, it's how we fetch the player file, and actually do anything, it also checks if the item pool exists
		/// </summary>
		/// <returns>A new player, or auto loads a player</returns>
		private async Task<HpPlayer> GetPlayer(IUser user)
		{
			//HACK: Find a better way that uses time or something. Every 8'th message is kinda not that good
			savecounter++;
			if (savecounter > 8)
			{
				SaveAllPlayers();
				savecounter = 0;
			}
			HpPlayer p = null;

			if (PlayerRamMemory.ContainsKey(user.Id)) //if it already is loaded
			{
				p = PlayerRamMemory[user.Id];
			}
			else //fetch from slow storage
			{
				try
				{
					p = (await FileManager.LoadData<HpPlayer>(user.Id.ToString())).FirstOrDefault();
				}
				catch (Exception e)
				{
					await Logging.LogCriticalMessage("HPD", $"Failed to load player {e.Message}");
				}

				//if it's null, we make a new one, and init it
				if (p == null)
				{
					await Logging.LogInfoMessage("HPD", $"Creating player {user.Id}");
					Console.WriteLine("New player: " + user.Id.ToString());
					p = new HpPlayer(user.Id, default)
					{
						Id = user.Id,
						Skills = new Dictionary<string, int>(),
						Name = user.Username
					};
					SavePlayerDisk(p); //make base save aswell.
				}
				//add to ram
				PlayerRamMemory.Add(user.Id, p);
			}

			return p;
		}


		//
		/// <summary>
		/// Stores the player in the ram module
		/// </summary>
		/// <param name="player">Target player to store</param>
		/// <returns></returns>
		private async void SavePlayer(HpPlayer player)
		{
			await Logging.LogInfoMessage("HPD", $"Saving players");
			if (PlayerRamMemory.ContainsKey(player.Id))
				PlayerRamMemory[player.Id] = player;
			else
				PlayerRamMemory.Add(player.Id, player);
		}

		/// <summary>
		/// Saves all players from ram to Disk
		/// </summary>
		/// <returns>Fuck all</returns>
		private void SaveAllPlayers()
		{
			foreach (var p in PlayerRamMemory)
			{
				SavePlayerDisk(p.Value);
			}
			Console.WriteLine("All players saved");
		}

		/// <summary>
		/// Stores a single player to disk from ram
		/// </summary>
		/// <param name="player">Target player</param>
		/// <returns>Nothing at all</returns>
		private void SavePlayerDisk(HpPlayer player)
		{
			//yes i'm this lazy
			List<HpPlayer> p = new List<HpPlayer>
			{
				player
			};
			FileManager.SaveData(p, player.Id.ToString());
		}


		#endregion

		[Command("hpdsave")]
		[Summary("Saves all players, only DM")]
		[RequireContext(ContextType.DM)]
		public async Task HpdSavePlayers()
		{
			if (_user.ByDiscordId(Context.User.Id).HasPermission(Enums.UserPermissions.Permissions.HeadpatDungeonHDPSave))
			{
				await ReplyAsync(_module.HPDSavePlayers());
				return;
			}
			await ReplyAsync("You're not allowed to manually run the save command");
		}

		[Command("mine")]
		[Summary("Go gather ores")]
		public async Task Mine([Remainder] string orename = null)
		{
			await ReplyAsync(_module.Action("mine", Context.User.Id, orename));
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

			foreach (var item in p.Inventory) //TODO: Find a better padding method, discord hates whitespaces, and '\u202F' is not pretty..
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
		public async Task Craft([Remainder] string craft)
		{
			//check if whatever the fuck they typed, is a key
			if (_crafting.GetRecepies().GetRecipesList().ContainsKey(craft.ToLower()))
			{
				var i = _crafting.GetRecepies().GetRecipesList()[craft.ToLower()];
				HpPlayer p = await GetPlayer(Context.User);

				bool levelup = false;                                      //Prepare to check if they got a lvl
				int currlvl = p.GetSkillLevel(i.Skill);                     //Remember current lvl
				var crafted = _crafting.Craft(craft.ToLower(), ref p);
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
		}
	}
}