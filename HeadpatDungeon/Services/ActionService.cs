using HeadpatDungeon.Contracts;
using HeadpatDungeon.Models;
using HeadpatDungeon.Models.Entities;

using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace HeadpatDungeon.Services
{
    public class ActionService : IActionService
    {
        private readonly Random rng = new Random();
        private readonly IPlayerService _playerServ;
        private readonly IItemContainer _itemContainer;

        public ActionService(IPlayerService playerServ, IItemContainer itemContainer)
        {
            _playerServ = playerServ;
            _itemContainer = itemContainer;
        }

        public string ExecuteAction(string action, HpPlayer player, string actionObject = null)
        {
            switch(action)
            {
                case "agility":
                    return TrainAgility(player);
                case "strength":
                    return TrainStrength(player);
                case "mine":
                    return Mine(player, actionObject);
                case "chop":
                    return Chop();
                default:
                    return DefaultText(action);
            }
        }

        #region Skills

        private string TrainAgility(HpPlayer player)
        {
            int gainedEXP = rng.Next(10, 200);
            bool levelUp = player.AddXPToSkill("agility", gainedEXP);

            _playerServ.SavePlayer(player);

            StringBuilder response = new StringBuilder();
            response.Append($"You did some running and gained {gainedEXP}XP");

            if(levelUp)
            {
                response.Append(LevelUpText("agility", player));
            }
            return response.ToString();
        }

        private string TrainStrength(HpPlayer player)
        {
            int gainedEXP = rng.Next(10, 200);
            bool levelUp = player.AddXPToSkill("strength", gainedEXP);

            _playerServ.SavePlayer(player);

            StringBuilder response = new StringBuilder();
            response.Append($"You did some pushups and gained {gainedEXP}XP");

            if (levelUp)
            {
                response.Append(LevelUpText("strength", player));
            }
            return response.ToString();
        }

        #endregion

        #region Actions

        private string Mine(HpPlayer player, string oreName)
        {
            HpItem item = null;
            if(!string.IsNullOrEmpty(oreName))
            {
                oreName = oreName.ToLower();
                try
                {
                    _itemContainer.GetOreList().TryGetValue(oreName, out item);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                if (item != null && item.ItemLevel > player.GetSkillLevel("mining"))
                    item = null;
            }

            if (item == null)
            {
                item = _itemContainer.GetOreList().ElementAt(rng.Next(_itemContainer.GetOreList().Count)).Value;
                while (item.ItemLevel > player.GetSkillLevel("mining"))
                    item = _itemContainer.GetOreList().ElementAt(rng.Next(_itemContainer.GetOreList().Count)).Value;
            }

            player.AddItem(item);
            bool levelUp = player.AddXPToSkill("mining", item.ItemXp);

            _playerServ.SavePlayer(player);

            StringBuilder response = new StringBuilder();
            response.Append($"You've gone mining and have obtained **{item.ItemName}** \r\nand gained {item.ItemXp}XP");

            if (levelUp)
            {
                response.Append(LevelUpText("mining", player));
            }
            return response.ToString();
        }

        private string Chop()
        {
            throw new NotImplementedException();
        }

        #endregion

        private string LevelUpText(string skill, HpPlayer player)
        {
            return $"\r\nYou gained a **{skill} level!** \r\n Your level is now **{player.GetSkillLevel(skill)}**";
        }

        private string DefaultText(string skill)
        {
            return $"You looked in the sky and concentrated really hard on {skill}... Nothing happened.";
        }
    }
}
