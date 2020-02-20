using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;

using TastyBot.FutureHeadPats;
using TastyBot.Services;

namespace TastyBot.Modules
{
	/// <summary>
	/// The Module that handles all FHP related interactions
	/// </summary>
	[Name("FutureHeadpat")]
	public class FhpModule : ModuleBase<SocketCommandContext>
	{
		private HeadpatService _HeadpatService;

		public FhpModule(HeadpatService headpatService)
		{
			_HeadpatService = headpatService;
		}

		/// <summary>
		/// Enables Earan to hand out freshly printed headpats
		/// </summary>
		/// <param name="receiveUser">The user that will receive the FHP</param>
		/// <param name="amount">The amount of FHP the user will receive.</param>
		/// <returns></returns>
		[Command("give")]
		[Summary("Hands out headpats to the user")]
		public async Task Give(IUser receiveUser, long amount)
		{
			if (Context.User.Id != 277038010862796801)
			{
				await ReplyAsync("NO!");
				return;
			}

			FhpUser sender = _HeadpatService.GetUser(Context.User);
			FhpUser receiver = _HeadpatService.GetUser(receiveUser);

			receiver.Wallet += amount;

			await ReplyAsync($"{sender.Name} gave {receiver.Name} {amount}FHP");
		}

		/// <summary>
		/// Saves the dictionary to the json file.
		/// </summary>
		/// <returns></returns>
		[Command("save")]
		[Summary("Hands out headpats to the user")]
		public async Task Save()
		{
			if (Context.User.Id != 277038010862796801 || Context.User.Id != 83183880869253120)
			{
				await ReplyAsync("NO!");
				return;
			}
			_HeadpatService.Save();
			await ReplyAsync("Saved");
		}

		/// <summary>
		/// Gives FHP from one user to another user
		/// </summary>
		/// <param name="receiveUser">The user that receives the headpats</param>
		/// <param name="amount">The amount of headpats to transfer</param>
		/// <returns></returns>
		[Command("pat")]
		[Summary("headpat another person")]
		public async Task Pat(IUser receiveUser, uint amount = 1)
		{
			FhpUser sender = _HeadpatService.GetUser(Context.User);
			FhpUser receiver = _HeadpatService.GetUser(receiveUser);

			if (sender.Wallet < amount)
			{
				await ReplyAsync("Sadly you don't have enough headpats :sob:");
				return;
			}

			sender.Wallet -= amount;
			receiver.Wallet += amount;

			await ReplyAsync($"{sender.Name} patted {receiver.Name} and sent over {amount}FHP");
		}

		/// <summary>
		/// Prints out the current wallet either for the invoking, or the mentioned user
		/// </summary>
		/// <param name="otheruser">The user the wallet should be printed out for. If null the wallet of the invoking user is printed</param>
		/// <returns></returns>
		[Command("wallet")]
		[Summary("show your current wallet balance")]
		public async Task Wallet(IUser otheruser = null)
		{
			FhpUser user = _HeadpatService.GetUser(Context.User);
			if (otheruser != null)
			{
				user = _HeadpatService.GetUser(otheruser);
			}
			Random rColor = new Random((int)user.Wallet);
			var builder = new EmbedBuilder()
			{
				Color = new Color(rColor.Next(0, 256), rColor.Next(0, 256), rColor.Next(0, 256)),
				Title = $"{user.Name}'s Wallet"
			};
			builder.AddField("FHP", user.Wallet);
			await ReplyAsync(embed: builder.Build());
		}


		/// <summary>
		/// Prints out a leaderboard with all people that currently are tracked by the FHP Module
		/// </summary>
		/// <returns></returns>
		[Command("leaderboard")]
		[Summary("shows the current FHP leaderboard")]
		public async Task Leaderboard()
		{
			List<FhpUser> users = _HeadpatService.GetLeaderboard();
			StringBuilder leaderboardBuilder = new StringBuilder();

			var builder = new EmbedBuilder()
			{
				Color = new Color(0, 255, 0),
				Title = "Leaderboard"
			};

			string leaderboard = string.Empty;
			for (int i = 0; i < users.Count; i++)
			{
				leaderboard += $"{i + 1}. {users[i].Name} {users[i].Wallet} FHP\r\n";
			}
			leaderboard = leaderboard.TrimEnd('\n', '\r');

			builder.AddField("test", leaderboard);
			await ReplyAsync(embed: builder.Build());
		}
	}
}
