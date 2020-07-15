using MasterMind.Contracts;
using MasterMind.Entities;
using MasterMind.Modules;

using System.Collections.Generic;
using System.Linq;
using System.Timers;

using Discord;

namespace MasterMind.Services
{
    public class MasterMindService : IMasterMindService
    {
        private readonly IFileManagerMM _fileManager;
        private readonly Dictionary<ulong, MasterMindUser> _Users;
        private readonly Timer _SaveTimerr;

        public MasterMindService(IFileManagerMM fileManager)
        {
            _fileManager = fileManager;
            _Users = new Dictionary<ulong, MasterMindUser>();
            _SaveTimerr = new Timer()
            {
                AutoReset = true,
                Interval = 5 * 60 * 1000,
                Enabled = true,
            };
            _SaveTimerr.Elapsed += SaveTimer_Elapsed;
            List<MasterMindUser> users = _fileManager.LoadMasterMindUserData();
            foreach (var item in users)
            {
                _Users[item.Id] = item;
            }
        }

        private void SaveTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _fileManager.SaveMasterMindUserData(_Users.Values.ToList());
        }

        /// <summary>
        /// Returns the <see cref="MasterMindUser"/> for the given discord <paramref name="user"/>. If the corresponding <see cref="MasterMindUser"/> doesn't exist, it is created.
        /// </summary>
        /// <param name="user">The discord user to look up</param>
        /// <returns></returns>
        public MasterMindUser GetUser(IUser user)
        {
            if (!_Users.ContainsKey(user.Id))
            {
                MasterMindUser newUser = new MasterMindUser
                {
                    masterMindCommands = new MasterMindCommands(),
                    Id = user.Id,
                    Name = user.Username,
                    Tag = $"{user.Username}#{user.Discriminator}",
                    GameRunning = false,
                    AmountOfWins = 0,
                };
                _Users[newUser.Id] = newUser;
            }
            return _Users[user.Id];
        }

        /// <summary>
        /// Deletes the amount of wins of the <paramref name="user"/> if the user has one.
        /// </summary>
        /// <param name="user">The user which's amount of wins is going to be deleted</param>
        /// <returns></returns>
        public bool DeleteUser(IUser user)
        {
            if (_Users.ContainsKey(user.Id))
            {
                _Users.Remove(user.Id);
                return true;
            }
            return false;
        }

        
        /// <summary>
        /// Saves the current <see cref="MasterMindUser"/>s
        /// </summary>
        public void Save()
        {
            _fileManager.SaveMasterMindUserData(_Users.Values.ToList());
        }

        /// <summary>
        /// Returns a list ordered by descending amount of wins amount.
        /// </summary>
        /// <returns></returns>
        public List<MasterMindUser> GetLeaderboard()
        {
            List<MasterMindUser> users = _Users.Values.ToList();
            return users.Where(x => x.AmountOfWins != 0).OrderByDescending(x => x.AmountOfWins).ToList();
        }
    }
}
