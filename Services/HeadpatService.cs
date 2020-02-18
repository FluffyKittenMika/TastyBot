using System.Collections.Generic;
using System.Linq;
using System.Timers;

using Discord;

using TastyBot.FutureHeadPats;
using TastyBot.Utility;

namespace TastyBot.Services
{
	public class HeadpatService
	{
		private Dictionary<ulong, FhpUser> _Users;
		private Timer _SaveTimer;

		public HeadpatService()
		{
			_Users = new Dictionary<ulong, FhpUser>();
			_SaveTimer = new Timer()
			{
				AutoReset = true,
				Interval = 5 * 60 * 1000,
				Enabled = true,
			};
			_SaveTimer.Elapsed += _SaveTimer_Elapsed;
			List<FhpUser> users = FileManager.LoadFhpUserData();
			foreach (var item in users)
			{
				_Users[item.Id] = item;
			}
		}

		private void _SaveTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			FileManager.SaveFhpUserData(_Users.Values.ToList());
		}

		public FhpUser GetUser(IUser user)
		{
			if (!_Users.ContainsKey(user.Id))
			{
				FhpUser newUser = new FhpUser
				{
					Id = user.Id,
					Name = user.Username,
					Tag = $"{user.Username}#{user.Discriminator}",
					Wallet = 0
				};
				_Users[newUser.Id] = newUser;
			}
			return _Users[user.Id];
		}

		public void Save()
		{
			FileManager.SaveFhpUserData(_Users.Values.ToList());
		}

		public List<FhpUser> GetLeaderboard()
		{
			List<FhpUser> users = _Users.Values.ToList();
			return users.OrderByDescending(x => x.Wallet).ToList();
		}
	}
}
