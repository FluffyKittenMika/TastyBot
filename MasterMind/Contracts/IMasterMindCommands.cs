using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MasterMind.Contracts
{
    public interface IMasterMindCommands
    {
        MemoryStream StartBoardMaker(int height, int width);
        void MakePattern();
        void StartGame();
        MemoryStream Game(List<int> colorGuessEmotes);

    }
}
