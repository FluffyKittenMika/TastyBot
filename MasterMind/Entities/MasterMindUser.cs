using MasterMind.Modules;
using System;
using System.Collections.Generic;
using System.Text;
using MasterMind.Contracts;

namespace MasterMind.Entities
{
    public class MasterMindUser
    {
        
        public string Tag { get; set; }
        public ulong Id { get; set; }
        public string Name { get; set; }
        public bool GameRunning { get; set; }
        public int AmountOfWins { get; set; }

        public MasterMindCommands masterMindCommands { get; set; }

    }
}
