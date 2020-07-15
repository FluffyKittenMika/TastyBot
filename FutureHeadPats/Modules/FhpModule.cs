using Discord;
using FutureHeadPats.Contracts;
using FutureHeadPats.Entities;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;

using Enums.UserPermissions;
using Interfaces.Contracts.BusinessLogicLayer;
using Interfaces.Entities.Models;
using Interfaces.Entities.ViewModels;

namespace FutureHeadPats.Modules
{
    public class FhpModule : IFhpModule
    {
        private readonly IHeadpatService _serv;
        private readonly IUserRepository _repo;

        public FhpModule(IHeadpatService serv, IUserRepository repo)
        {
            _serv = serv;
            _repo = repo;
        }

        public void Give(IUser receiver, long amount)
        {
            _serv.GetUser(receiver).Wallet += amount;
        }

        public string Delete(IUser deleteUser)
        {
            return _serv.DeleteUser(deleteUser) ? "Wallet deleted" : "User has no wallet";
        }

        public string Save()
        {
            _serv.Save();
            return "Saved";
        }

        public string Pat(IUser sender, IUser receiver, int amount = 1)
        {
            FhpUser senderFPH = _serv.GetUser(sender);
            FhpUser receiverFPH = _serv.GetUser(receiver);
            UserVM user = new UserVM(_repo.ByDiscordId(receiver.Id));

            if (receiver.IsBot)
                return "Can't pat a bot! Baka!";
            else if (user != null && user.HasPermission(Permissions.FutureHeadPatsAlmightyPatter))
                return "You cannot give headpats to the one giving out the headpats";

            if (amount < 0)
                return $"<@{senderFPH.Id}> tried to steal some pats from <@{receiverFPH.Id} but failed miserably.>";
            else if (amount == 0)
                return $"<@{senderFPH.Id}> tried to pat <@{receiverFPH.Id}> but lacked the resolve to do so.";
            else if (senderFPH.Wallet < amount)
                return "Sadly you don't have enough headpats :sob:";

            senderFPH.Wallet -= amount;
            receiverFPH.Wallet += amount;

            return $"<@{senderFPH.Id}> patted <@{receiverFPH.Id}> and sent over {amount}FHP";
        }

        public Embed Wallet(IUser user)
        {
            FhpUser userFHP = _serv.GetUser(user);

            Random rColor = new Random((int)userFHP.Wallet);
            var builder = new EmbedBuilder()
            {
                Color = new Color(rColor.Next(0, 256), rColor.Next(0, 256), rColor.Next(0, 256)),
                Title = $"{userFHP.Name}'s Wallet"
            };
            builder.AddField("FHP", userFHP.Wallet);
            return builder.Build();
        }

        public Embed Leaderboard()
        {
            List<FhpUser> users = _serv.GetLeaderboard();
            List<Embed> embeds = new List<Embed>();

            var builder = new EmbedBuilder()
            {
                Color = new Color(0, 255, 0),
                Title = "Leaderboard",
                Description = $"Total FHP: {users.Sum(x => x.Wallet)}"
            };

            StringBuilder leaderboard = new StringBuilder();
            string current = string.Empty;
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
        }

        public string Explain()
        {
            return "FHP stands for \"Future HeadPats\" and is the currency of this Server. " +
                "\r\n\r\n" +
                "At this moment you can do the following things:\r\n" +
                "!wallet\t\t\t\t\t\t\t\t - check your own wallet\r\n" +
                "!wallet @user\t\t\t\t\t- check somebody elses wallet\r\n" +
                "!leaderboard\t\t\t\t\t  - see the current FHP leaderboard\r\n" +
                "!pat @user <amount>\t - headpat other people and send them <amount> of headpats" +
                "\r\n\r\n" +
                "At some point in the future FHP holders will be able to turn them in for Headpats in VRC. There is no command for that yet.\r\n" +
                "The current exchange rate is 1FHP = 1 minute of uninterrupted headpatting.\r\n" +
                "The headpats are given out by <@277038010862796801>, up to 30 Minutes at a time." +
                "\r\n\r\n" +
                "Since <@277038010862796801> is a derp and gives out headpats for free to anyone showing up in VR, don't take this too seriously. It's just for funsies <:Tastyderp:669202378095984640>";
        }
    }
}