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
        Task StartGame();
        Task<MemoryStream> StartBoardMaker(int Height, int width);
        bool IsGameRunning();
    }
}
