using HeadpatDungeon.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace HeadpatDungeon.Models.Entities
{
    public class HpCreature : DefaultCreature
    {
        private int CurrentHP { get; set; }
        private readonly Random _rng;

        public HpCreature(DefaultCreature defaultCreature)
        {
            Name = defaultCreature.Name;
            Level = defaultCreature.Level;
            MaxHP = defaultCreature.MaxHP;
            CurrentHP = MaxHP;
            MinATK = defaultCreature.MinATK;
            MaxATK = defaultCreature.MaxATK;
            _rng = new Random();
            
        }

        public void Attack(HpCreature Target)
        {
            int att = _rng.Next(MinATK, MaxATK);
            Target.CurrentHP -= att;
        }

        public bool IsDead()
        {
            return CurrentHP <= 0;
        }

        public string GetName()
        {
            return Name;
        }

        public string GetInfo()
        {
            return $"{Name} Has {CurrentHP}/{MaxHP}HP and can do {MinATK}-{MaxATK}Dmg";
        }
    }
}
