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
	[Name("FutureHeadpat")]
	public class FhpModule : ModuleBase<SocketCommandContext>
	{
		private HeadpatService _HeadpatService;

		public FhpModule(HeadpatService headpatService)
		{
			_HeadpatService = headpatService;
		}

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

		[Command("save")]
		[Summary("Hands out headpats to the user")]
		public async Task Save()
		{
			if (Context.User.Id != 277038010862796801)
			{
				await ReplyAsync("NO!");
				return;
			}
			_HeadpatService.Save();
		}

		[Command("pat")]
		[Summary("headpat another person")]
		public async Task Pat(IUser receiveUser)
		{
			FhpUser sender = _HeadpatService.GetUser(Context.User);
			FhpUser receiver = _HeadpatService.GetUser(receiveUser);

			bool generated = false;

			if (sender.Wallet <= 0)
			{
				await ReplyAsync("Sadly you have no headpats to give :sob:");
				return;
			}

			sender.Wallet -= 1;
			receiver.Wallet += 1;

			if (generated)
			{
				await ReplyAsync($"{sender.Name} got lucky and generated one FHP for {receiver.Name}");
			}
			else
			{
				await ReplyAsync($"{sender.Name} patted {receiver.Name} and sent over 1FHP");
			}
		}

		[Command("wallet")]
		[Summary("show your current wallet balance")]
		public async Task Wallet()
		{
			FhpUser user = _HeadpatService.GetUser(Context.User);
			Random rColor = new Random((int)user.Wallet);
			var builder = new EmbedBuilder()
			{
				Color = new Color(rColor.Next(0, 256), rColor.Next(0, 256), rColor.Next(0, 256)),
				Title = $"{user.Name}'s Wallet"
			};
			builder.AddField("FHP", user.Wallet);
			await ReplyAsync(embed: builder.Build());
		}

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

			foreach (var item in users)
			{
				builder.AddField(item.Name, item.Wallet);
			}
			await ReplyAsync(embed: builder.Build());
		}
	}
}
