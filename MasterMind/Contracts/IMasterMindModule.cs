using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.Rest;
using Discord.WebSocket;

using MasterMind.Modules;


namespace MasterMind.Contracts
{
    public interface IMasterMindModule
    {
        Bitmap StartGame(int width, int height, IUser user, System.Drawing.Color colorIndicator);
        bool UserHasRunningGame(IUser user);
        Bitmap RunGame(Emoji CircleColor, IUser user);
        bool IsEmojiAllowed(Emoji emoji);
        Task DeleteMessage(int AmountOfTimeSec, RestUserMessage message);
        void SaveMessage(RestUserMessage userMessage, IUser user);
        bool ReactionOnRightMessage(ulong ReactionMessage, IUser user);
        bool IsAnArrow(Emoji emoji);
        bool CanArrowBeUsed(IUser user);
        bool IsSecondReactionAdded(ISocketMessageChannel socketMessage, SocketReaction socketReaction);
        bool DidUserWin(IUser user);
        bool DidUserLose(IUser user);
        long GetUserWins(IUser user);
    }
}
