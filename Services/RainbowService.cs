using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Discord;
using Discord.Rest;
using Discord.WebSocket;

namespace TastyBot.Services
{
	public class RainbowService
	{
		private readonly DiscordSocketClient _discord;
		private readonly Random _random;

		private readonly RestGuild _tastyGuild;
		private readonly HashSet<ulong> _tastyUsers = new HashSet<ulong>();
		private readonly RestRole _rainbowRole;
		private readonly RestRole TeamBlue;

		public RainbowService(DiscordSocketClient discord, Random random)
		{
			_discord = discord;
			_random = random;

			//we'll just change the role colors when we get a message on the discord, instead of doing it over time ;)
			_discord.MessageReceived += MessageReceivedAsync;

			_tastyGuild = _discord.Rest.GetGuildAsync(656909209547309062).Result;
			_rainbowRole = _tastyGuild.GetRole(671063987999342654);
			TeamBlue = _tastyGuild.GetRole(656953375862161418);

			InitHashset().Wait();

			_discord.UserJoined += _discord_UserJoined;
			_discord.UserLeft += _discord_UserLeft;
			_discord.UserBanned += _discord_UserBanned;
			_discord.UserUnbanned += _discord_UserUnbanned;

			//This is just to be fancy
			string rainbow = "started rainbow service";
			var count = Enum.GetNames(typeof(ConsoleColor)).Length;
			foreach (char c in rainbow)
			{
				Console.ForegroundColor = (ConsoleColor)typeof(ConsoleColor).GetEnumValues().GetValue(_random.Next(0, count));
				Console.Write(c);
			}
			Console.WriteLine();
			Console.ForegroundColor = ConsoleColor.Green; //ye we'll just keep it green m'kay? Hack the planet!
		}

		private Task _discord_UserUnbanned(SocketUser arg1, SocketGuild arg2)
		{
			return Task.Run(() =>
			{
				if (arg2.Id != _tastyGuild.Id)
				{
					return;
				}
				if (!_tastyUsers.Contains(arg1.Id))
				{
					_tastyUsers.Add(arg1.Id);
				}
			});
		}

		private Task _discord_UserBanned(SocketUser arg1, SocketGuild arg2)
		{
			return Task.Run(() =>
			{
				if (arg2.Id != _tastyGuild.Id)
				{
					return;
				}
				if (_tastyUsers.Contains(arg1.Id))
				{
					_tastyUsers.Remove(arg1.Id);
				}
			});
		}

		private Task _discord_UserLeft(SocketGuildUser arg)
		{
			return Task.Run(() =>
			{
				if (arg.Guild.Id != _tastyGuild.Id)
				{
					return;
				}
				if (_tastyUsers.Contains(arg.Id))
				{
					_tastyUsers.Remove(arg.Id);
				}
			});
		}

		private Task _discord_UserJoined(SocketGuildUser arg)
		{
			return Task.Run(() =>
			{
				if (arg.Guild.Id != _tastyGuild.Id)
				{
					return;
				}
				if (!_tastyUsers.Contains(arg.Id))
				{
					_tastyUsers.Add(arg.Id);
				}
			});
		}

		private async Task InitHashset()
		{
			IEnumerable<RestGuildUser> users = await _tastyGuild.GetUsersAsync().FlattenAsync();
			_tastyUsers.Clear();
			foreach (RestUser user in users)
			{
				_tastyUsers.Add(user.Id);
			}
		}

		private async Task MessageReceivedAsync(SocketMessage arg)
		{
			// Ignore system messages, or messages from other bots
			if (!(arg is SocketUserMessage message))
			{
				return;
			}
			if (message.Source != MessageSource.User)
			{
				return;
			}
			if (!_tastyUsers.Contains(message.Author.Id))
			{
				return;
			}

			await _rainbowRole.ModifyAsync(x =>
			{
				x.Color = Rainbow();
			});
			await TeamBlue.ModifyAsync(x =>
			{
				x.Color = BlueShades();
			});
		}
		private Color BlueShades()
		{
			int b = _random.Next(1, 255); //starting from 1 cuz if its 0 the discord decides to use another role color for the name, dunno y;
			return new Color(0, 0, b);
		}
		private Color Rainbow()
		{
			int r = _random.Next(0, 255);
			int g = _random.Next(0, 255);
			int b = _random.Next(0, 255);
			return new Color(r, g, b);
		}
	}
}
