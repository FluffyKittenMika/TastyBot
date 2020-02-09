using System;
using System.Collections.Generic;
using System.Text;

namespace TastyBot.HpDungeon
{
    /// <summary>
    /// This is the basic Class for ANYTHING LIVING
    /// That includes BIPEDALS, CATS, DOGS, ORCS, HUMANS, They DO NOT SHARE EQUIPMENT SLOTS.
    /// THEY DO NOT NEED EQUIPMENT SLOTS.
    /// </summary>
    public class HpCreature
    {
        private int _maxHP;
        private int _currentHP;
        private string _Name;
        private int _minAttack;
        private int _maxAttack;
        private int _level;

        //TODO: move random into a static global area
        readonly Random rng = new Random();

        /// <summary>
        /// Constructor
        /// </summary>
        public HpCreature(string name = "No name", int maxhp = 100,int level = 1, int minattack = 1, int maxattack = 10)
        {
            _Name = name;
            _maxHP = maxhp;
            _currentHP = _maxHP;
            _minAttack = minattack;
            _maxAttack = maxattack;
            _level = level;
        }

        /// <summary>
        /// Attack a target, and apply damage based on this creatures damage
        /// </summary>
        /// <param name="Target"></param>
        public void Attack(HpCreature Target)
        {
            int att = rng.Next(_minAttack, _maxAttack);
            Target._currentHP -= att;
        }

        /// <summary>
        /// Returns True if it's dead.
        /// </summary>
        /// <returns>True if dead</returns>
        public bool IsDead()
        {
            if (_currentHP <= 0)
                return true;
            return false;
        }

        /// <summary>
        /// Get the name of the Creature
        /// </summary>
        /// <returns>Name</returns>
        public string GetName()
        {
            return _Name;
        }

        /// <summary>
        /// Returns a bit more debuggable description
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{_Name} Has {_currentHP}/{_maxHP}HP and can do {_minAttack}-{_maxAttack}Dmg";
        }
    }
}
