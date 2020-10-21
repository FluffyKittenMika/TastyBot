using FutureHeadPats.Contracts;
using Enums.UserPermissions;

using Discord;
using Discord.Commands;

using System;
using System.Threading.Tasks;

using Interfaces.Contracts.BusinessLogicLayer;
using Interfaces.Entities.Models;
using Utilities.LoggingService;
using Interfaces.Entities.ViewModels;

namespace DiscordUI.Modules
{
    /// <summary>
    /// The Module that handles all FHP related interactions
    /// </summary>
    [Name("FutureHeadpat")]
    public class ModuleFHP : ModuleBase<SocketCommandContext>
    {
        private readonly IFhpModule _module;
        private readonly IUserService _serv;

        public ModuleFHP(IFhpModule module, IUserService serv)
        {
            _module = module;
            _serv = serv; 
        }

        /// <summary>
        /// Enables Earan to hand out freshly printed headpats
        /// </summary>
        /// <param name="userReceive">The user that will receive the FHP</param>
        /// <param name="amount">The amount of FHP the user will receive.</param>
        [Command("give")]
        [Summary("Hands out headpats to the user")]
        public async Task Give(IUser userReceive, long amount)
        {
            UserVM user = new UserVM(_serv.ByDiscordId(Context.User.Id));
            if (user != null && user.HasPermission(Permissions.FutureHeadPatsGive))
            {
                _module.Give(userReceive, amount);
                string given = (amount < 0) ? "taken from" : "given to";
                await ReplyAsync($"{Math.Abs(amount)} has been {given} {userReceive.Username}");
                return;
            }
            await ReplyAsync("NO!");
        }

        /// <summary>
        /// Deletes the wallet of <paramref name="userDelete"/> if the wallet exists
        /// </summary>
        /// <param name="userDelete">The user which's wallet shall be deleted</param>
        /// <returns></returns>
        [Command("delete")]
        [Summary("Deletes the wallet of the tagged user, if the user has a wallet")]
        public async Task Delete(IUser userDelete)
        {
            UserVM user = new UserVM(_serv.ByDiscordId(Context.User.Id));
            if (user != null && user.HasPermission(Permissions.FutureHeadPatsDelete))
            {
                await ReplyAsync(_module.Delete(userDelete));
                return;
            }
            await ReplyAsync("NO!");

        }

        /// <summary>
        /// Saves the dictionary to the json file.
        /// </summary>
        /// <returns></returns>
        [Command("save")]
        [Summary("Instantly saves the current users")]
        public async Task Save()
        {
            UserVM user = new UserVM(_serv.ByDiscordId(Context.User.Id));
            if (user.Administrator || user.HasPermission(Permissions.FutureHeadPatsSave))
            {
                await ReplyAsync(_module.Save());
                return;
            }
            await ReplyAsync("NO!");
        }

        /// <summary>
        /// Gives FHP from one user to another user
        /// </summary>
        /// <param name="userReceive">The user that receives the headpats</param>
        /// <param name="amount">The amount of headpats to transfer</param>
        /// <returns></returns>
        [Command("pat")]
        [Summary("Headpat another person")]
        public async Task Pat(IUser userReceive, int amount = 1)
        {
            await ReplyAsync(_module.Pat(Context.User, userReceive, amount));
        }

        /// <summary>
        /// Prints out the current wallet either for the invoking, or the mentioned user
        /// </summary>
        /// <param name="differentUser">The user the wallet should be printed out for. If null the wallet of the invoking user is printed</param>
        /// <returns></returns>
        [Command("wallet")]
        [Summary("Show your current wallet balance")]
        public async Task Wallet(IUser differentUser = null)
        {
            IUser user = differentUser;
            if (user == null)
            {
                user = Context.User;
            }
            await ReplyAsync(embed: _module.Wallet(user));
        }

        /// <summary>
        /// Prints out a leaderboard with all people that currently are tracked by the FHP Module
        /// </summary>
        /// <returns></returns>
        [Command("leaderboard")]
        [Summary("Shows the current FHP leaderboard")]
        public async Task Leaderboard()
        {
            await ReplyAsync(embed: _module.Leaderboard());
        }

        [Command("explain")]
        [Summary("Explains what FHP means")]
        public async Task Explain()
        {
            await ReplyAsync(_module.Explain());
        }
    }
}
