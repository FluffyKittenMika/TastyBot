using Discord;

using FileManager.Contracts;
using HeadpatDungeon.Contracts;
using HeadpatDungeon.Models.Entities;
using System;
using System.Collections.Generic;

namespace HeadpatDungeon.Modules
{
    public class HpDModule : IHpDModule
    {
        private readonly IFileManager _fileManager;
        private readonly IPlayerService _playerServ;
        private readonly IActionService _actionServ;

        public HpDModule(IFileManager fileManager, IPlayerService playerServ, IActionService actionServ)
        {
            _fileManager = fileManager;
            _playerServ = playerServ;
            _actionServ = actionServ;

        }

        /// <summary>
		/// Save all players, administrator permissions required.
		/// </summary>
		/// <returns>String</returns>
        public string HPDSavePlayers()
        {
            _playerServ.SaveAllPlayers();
            return "Saved";
        }

        /// <summary>
		/// Actions that a player performs: chopping wood, mining etc.
		/// </summary>
        /// <param name="action">Target action</param>
        /// <param name="id">Target action</param>
		/// <returns>String</returns>
        public string Action(string action, ulong id)
        {
            HpPlayer player = _playerServ.GetPlayer(id);
            return _actionServ.ExecuteAction(action, player);
        }

        /// <summary>
		/// Actions that a player performs: chopping wood, mining etc.
		/// </summary>
        /// <param name="action">Target action</param>
		/// <returns>String</returns>
        public string Craft(string craft)
        {
            throw new NotImplementedException();
        }

        /// <summary>
		/// A showcase of all the items in the players inventory.
		/// </summary>
		/// <returns>Embed Inventory</returns>
        public Embed Inventory()
        {
            throw new NotImplementedException();
        }

        /// <summary>
		/// A showcase of the skillset from the given user.
		/// </summary>
        /// <param name="user">Target player</param>
		/// <returns>Embed Skillset</returns>
        public Embed Skills(IUser user)
        {
            throw new NotImplementedException();
        }
    }
}
