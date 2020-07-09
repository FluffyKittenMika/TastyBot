using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace MasterMind.Contracts
{
    public interface IMasterMindModule
    {
        void StartGame();
        MemoryStream StartBoardMaker(int Height, int width);
        bool IsGameRunning();
    }
}
