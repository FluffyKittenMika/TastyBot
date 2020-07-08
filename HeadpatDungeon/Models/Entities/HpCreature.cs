
using System;

namespace HeadpatDungeon.Models.Entities
{
    public class HpCreature
    {
        private int CurrentHP { get; set; }
        private readonly Random _rng;



        public HpCreature()
        {
            Name = "";
            Level = GetLevel(); //Level is based upon attack, defence and strenght
                                //Same goes with Min and max attack. Min is always 0, max is based on strenght and equipment.
            MaxHP = defaultCreature.MaxHP; //Hp is soley based on Health level
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
