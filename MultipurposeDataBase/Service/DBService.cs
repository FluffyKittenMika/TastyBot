using MultipurposeDataBase.Contracts;
using MultipurposeDataBase.HelperClasses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using Discord;
using System.Linq;
using MasterMind.HelperClasses;

namespace MultipurposeDataBase.Service
{
    public class DBService : IDBService
    {
        private readonly IFileManager _fileManager;
        private readonly Dictionary<ulong, MDBUser> _Users;
        private readonly Timer _SaveTimer;

        public DBService(IFileManager fileManager)
        {
            _fileManager = fileManager;
            _Users = new Dictionary<ulong, MDBUser>();
            _SaveTimer = new Timer()
            {
                AutoReset = true,
                Interval = 5 * 60 * 1000,
                Enabled = true,
            };
            _SaveTimer.Elapsed += SaveTimer_Elapsed;
            List<MDBUser> users = _fileManager.LoadMDBData();
            foreach (var item in users)
            {
                _Users[item.UserId] = item;
            }
        }

        private void SaveTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _fileManager.SaveMDBData(_Users.Values.ToList());
        }

        
        public MDBUser GetUser(IUser user)
        {
            
            if (!_Users.ContainsKey(user.Id))
            {
                MDBUser newUser = new MDBUser
                {
                    UserId = user.Id,
                    UserName = user.Username,
                    MMGame = new MasterMindDBUser(),

                };
                _Users[newUser.UserId] = newUser;
            }
            return _Users[user.Id];
        }

        
        public bool DeleteUser(IUser user)
        {
            if (_Users.ContainsKey(user.Id))
            {
                _Users.Remove(user.Id);
                return true;
            }
            return false;
        }

        
        public void Save()
        {
            _fileManager.SaveMDBData(_Users.Values.ToList());
        }

        /*
        public List<MDBUser> GetLeaderboard()
        {
            List<MDBUser> users = _Users.Values.ToList();
            return users.Where(x => x.Wallet != 0).OrderByDescending(x => x.Wallet).ToList();
        }
        */
        
    }
}
