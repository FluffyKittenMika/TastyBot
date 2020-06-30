using FutureHeadPats.Contracts;

using Discord;
using Discord.Commands;

using System;
using System.Threading.Tasks;
using Authorization.Contracts;

namespace TastyBot.Modules
{
    /// <summary>
    /// The Module that handles all FHP related interactions
    /// </summary>
    [Name("FutureHeadpat")]
    public class ModuleFHP : ModuleBase<SocketCommandContext>
    {
        private readonly IFhpModule _module;
        private readonly IPermissionHandler _permissionHandler;

        public ModuleFHP(IFhpModule module, IPermissionHandler permissionHandler)
        {
            _module = module;
            _permissionHandler = permissionHandler;
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
            if (!_permissionHandler.IsAdministrator(Context.User.Id))
            {
                await ReplyAsync("NO!");
                return;
            }
            _module.Give(userReceive, amount);
            string given = (amount < 0) ? "taken from" : "given to";
            await ReplyAsync($"{Math.Abs(amount)} has been {given} {userReceive.Username}");
        }

        /// <summary>
        /// Deletes the wallet of <paramref name="deleteUser"/> if the wallet exists
        /// </summary>
        /// <param name="deleteUser">The user which's wallet shall be deleted</param>
        /// <returns></returns>
        [Command("delete")]
        [Summary("Deletes the wallet of the tagged user, if the user has a wallet")]
        public async Task Delete(IUser userDelete)
        {
            if(!_permissionHandler.IsAdministrator(Context.User.Id))
            {
                await ReplyAsync("NO!");
                return;
            }
            await ReplyAsync(_module.Delete(userDelete));
        }

        /// <summary>
        /// Saves the dictionary to the json file.
        /// </summary>
        /// <returns></returns>
        [Command("save")]
        [Summary("Instantly saves the current users")]
        public async Task Save()
        {
            if (!_permissionHandler.IsAdministrator(Context.User.Id))
            {
                await ReplyAsync("NO!");
                return;
            }
            await ReplyAsync(_module.Save());
        }

        /// <summary>
        /// Gives FHP from one user to another user
        /// </summary>
        /// <param name="receiveUser">The user that receives the headpats</param>
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
            if(user == null)
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
