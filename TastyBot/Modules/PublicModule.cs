using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordUI.Modules
{
    // Modules must be public and inherit from an IModuleBase
    [Name("General Commands")]
    public class PublicModule : ModuleBase<SocketCommandContext>
    {
        // Ban a user
        [Command("ban")]
        [RequireContext(ContextType.Guild)]
        // make sure the user invoking the command can ban
        [RequireUserPermission(GuildPermission.BanMembers)]
        // make sure the bot itself can ban
        [RequireBotPermission(GuildPermission.BanMembers)]
        public async Task BanUserAsync(IGuildUser user, [Remainder] string reason = null)
        {
            await user.Guild.AddBanAsync(user, reason: reason);
            await ReplyAsync("ok!");
        }


        // Throw Back Thursday, Tasty Specified command.
        [Command("tbt")]
        [Summary("Use this command once every thursday :D")]
        public Task ThrowBackThursday()
            => ReplyAsync("<:Tastyderp:669202378095984640> Throwback Thursday! Post an old picture of you and your friends in #Photos!");
        
        [Command("inrole")]
        [Summary("Find out the users that have a specified role\t!inrole {role}")]
        public async Task Inrole([Remainder] string roleName)
        {
            
            bool Exist = false;
            List<SocketGuildUser> Lusers = new List<SocketGuildUser>();
            if (roleName == null)
            {
                await Context.Channel.SendMessageAsync($"<@{Context.User.Id}> Please specify a role");
                return;
            }
            foreach (IRole role in ((IGuildChannel)Context.Message.Channel).Guild.Roles)
            {
                if (role.Name.ToLower() == roleName)
                {
                    Exist = true;
                    var users = Context.Guild.Users;
                    foreach (var user in Context.Guild.Users)
                    {
                        foreach (var rrole in user.Roles)
                        {
                            if (rrole.Name.ToLower() == roleName)
                            {
                                Lusers.Add(user);
                                break;
                            }
                        }
                    }
                }
            }
            if (!Exist)
            {
                await Context.Channel.SendMessageAsync($"<@{Context.User.Id}> Specified role doesnt exist");
                return;
            }
            Random rnd = new Random();
            var builder = new EmbedBuilder()
            {
                Color = new Color(rnd.Next(1,255), rnd.Next(1, 255), rnd.Next(1, 255)),
                Title = $"Users in {roleName}",
                Description = $"Users in {roleName}",
            };
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < Lusers.Count; i++)
            {
                var current = $"<@{Lusers[i].Id}>";
                stringBuilder.AppendLine(current);
            }
            builder.AddField((builder.Fields.Count + 1).ToString(), stringBuilder.ToString().TrimEnd('\n', '\r'));
            await ReplyAsync(embed: builder.Build());

        }
        /*
         for (int i = 0; i < users.Count; i++)
            {
                current = $"{i + 1}. {users[i].Name} {users[i].Wallet} FHP";
                if (leaderboard.Length + current.Length >= 1024)
                {
                    builder.AddField((builder.Fields.Count + 1).ToString(), leaderboard.ToString().TrimEnd('\n', '\r'));
                    leaderboard.Clear();
                }
                leaderboard.AppendLine(current);
            }
            builder.AddField((builder.Fields.Count + 1).ToString(), leaderboard.ToString().TrimEnd('\n', '\r'));
            return builder.Build();
         */
        //foreach (IRole role in ((IGuildChannel)socketMessage.Channel).Guild.Roles)
        [Command("nnm")]
        [Summary("Use this command once very monday :D")]
        public Task NomNomMonday()
            => ReplyAsync("<:Oposum:669676896270680104> Nom Nom Monday! Post a picture of your food in #Photos!");

        // Get info on a user, or the user who invoked the command if one is not specified
        [Command("userinfo")]
        public async Task UserInfoAsync(IUser user = null)
        {
            user ??= Context.User;
            await ReplyAsync(user.ToString());
        }

        /* Commenting out theese commands as they're really not needed, keeping them around for reference. 
		 		
		// [Remainder] takes the rest of the command's arguments as one argument, rather than splitting every space
		[Command("echo")]
		public Task EchoAsync([Remainder] string text)
			// Insert a ZWSP before the text to prevent triggering other bots!
			=> ReplyAsync('\u200B' + text);
		

		// 'params' will parse space-separated elements into a list
		[Command("list")]
		public Task ListAsync(params string[] objects)
			=> ReplyAsync("You listed: " + string.Join("; ", objects));

		// Setting a custom ErrorMessage property will help clarify the precondition error
		[Command("guild_only")]
		[RequireContext(ContextType.Guild, ErrorMessage = "Sorry, this command must be ran from within a server, not a DM!")]
		public Task GuildOnlyCommand()
			=> ReplyAsync("Nothing to see here!");
		*/
    }
}