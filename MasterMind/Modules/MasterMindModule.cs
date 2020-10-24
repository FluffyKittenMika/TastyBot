using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Linq;

using Discord;
using Discord.WebSocket;
using Discord.Rest;

using MasterMind.Contracts;
using MasterMind.Entities;
using MasterMind.Modules;
using MasterMind.HelperClasses;

using Interfaces.Contracts.BusinessLogicLayer;
using BusinessLogicLayer.Services;

using Color = System.Drawing.Color;
using Interfaces.Entities.Models;


namespace MasterMind.Modules
{
    public class ReactionMemory
    {
        public ISocketMessageChannel socketMessages;
        public SocketReaction socketReactions;
    }

    public class MasterMindModule : IMasterMindModule
    {
        private readonly IMasterMindCacheService _mMCache;
        private readonly IMasterMindFunctions _functions;
        private List<ReactionMemory> reactionMemories;
        private readonly IMasterMindDataBase _serv;
        //MMuserService Doest work
        public MasterMindModule(IMasterMindCacheService mMCache, IMasterMindFunctions functions, IMasterMindDataBase service)
        {
            _mMCache = mMCache;
            _functions = functions;
            _serv = service;
        }
        
        public async Task DeleteMessage(int AmountOfTimeSec, RestUserMessage message)
        {
            await Task.Delay(AmountOfTimeSec * 1000);
            await message.DeleteAsync();
        }

        public bool UserHasRunningGame(IUser user)
        {
            if (_mMCache.CacheExists(user.Id.ToString()))
            {
                if (_mMCache.RetrieveItem<MMUserCache>(user.Id.ToString()).CurrentGameRunning)
                {
                    return true;

                }

            }
            return false;
        }

        public long GetUserWins(IUser user)
        {
            return _serv.GetMMDBUser(user).wins;
        }

        /*
        public Bitmap StartGame(int width, int height, IUser user)
        {
            MMUserCache userCache = new MMUserCache();
            if (_mMCache.CacheExists(user.Id.ToString())) //checks if cache exists
            {
                if (userCache.CurrentGameRunning) return null;
                _mMCache.RemoveCache(user.Id.ToString());
            }

            userCache.CurrentGameRunning = true;
            userCache.DotsInHeight = height;
            userCache.DotsInWidth = width;
            userCache.bitPicture = _functions.StartBoardMaker(height, width);
            userCache.Lines = new List<Line>();
            for (int i = 0; i < height; i++)
            {
                Line line = new Line();
                line.LineNum = i;
                userCache.Lines.Add(line);
            }
            _mMCache.StoreItems(userCache, user.Id.ToString());
            return userCache.bitPicture;
        }
        */
        public bool IsEmojiAllowed(Emoji emoji)
        {
            Color TestColor = _functions.EmojiToColor(emoji);
            if (TestColor.A == 0 && TestColor.R == 0 && TestColor.G == 0 && TestColor.B == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void SaveMessage(RestUserMessage userMessage, IUser user)
        {
            MMUserCache userCache = _mMCache.RetrieveItem<MMUserCache>(user.Id.ToString());
          
            userCache.Messages.Add(userMessage);
            _mMCache.RemoveCache(user.Id.ToString());
            _mMCache.StoreItems(userCache, user.Id.ToString());
        }

        public bool IsAnArrow(Emoji emoji)
        {
            Color TestColor = _functions.EmojiToColor(emoji);
            if (TestColor.A == 1 && TestColor.R == 1 && TestColor.G == 1 && TestColor.B == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CanArrowBeUsed(IUser user)
        {
            MMUserCache userCache = _mMCache.RetrieveItem<MMUserCache>(user.Id.ToString());
            

            if (userCache.DotColumnPostion  >= (userCache.DotsInWidth - 1)) //The minus 1 is there so that both start off at 0 instead of 1
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }

        public bool DidUserWin(IUser user)
        {
            
            MMUserCache userCache = _mMCache.RetrieveItem<MMUserCache>(user.Id.ToString());
            for (int i = 0; i < userCache.DotsInWidth; i++)
            {
                if (userCache.Lines[userCache.CurrentLine].ColorGuessNumber[i] != userCache.SecretPattern[i])
                {
                    return false;
                }
            }
            userCache.CurrentGameRunning = false;
            
            _serv.GetMMDBUser(user).wins++;
            return true;
        }

        public bool DidUserLose(IUser user)
        {
            MMUserCache userCache = _mMCache.RetrieveItem<MMUserCache>(user.Id.ToString());
            if ((userCache.CurrentLine + 1) == userCache.DotsInHeight)
            {
                userCache.CurrentGameRunning = false;
                return true;
            }
            return false;
        }

        public Bitmap RunGame(Emoji CircleEmoji, IUser user)
        {
            //it can be an arrow
            MMUserCache userCache = _mMCache.RetrieveItem<MMUserCache>(user.Id.ToString());
            if (IsAnArrow(CircleEmoji))
            {
                userCache.Lines[userCache.CurrentLine].RedAmount = _functions.RedNum(userCache.Lines[userCache.CurrentLine].ColorGuessNumber, userCache.SecretPattern);
                userCache.Lines[userCache.CurrentLine].WhiteAmount = _functions.WhiteNum(userCache.Lines[userCache.CurrentLine].ColorGuessNumber, userCache.SecretPattern, userCache.DotsInWidth);
                userCache.bitPicture = _functions.RedAndWhiteDotEditor(userCache.ImageGraphics, userCache.bitPicture, userCache.DotsInWidth, userCache.Lines[userCache.CurrentLine].RedAmount, userCache.Lines[userCache.CurrentLine].WhiteAmount, userCache.CurrentLine);
                userCache.CurrentLine += 1;
                userCache.DotColumnPostion = 0;
            }
            else
            {
                int CurrentColumnPosition = userCache.DotColumnPostion;
                do
                {
                    CurrentColumnPosition -= userCache.DotsInWidth;
                } while (CurrentColumnPosition >= 0);
                CurrentColumnPosition += userCache.DotsInWidth;

                Line CurrentLinee = userCache.Lines.ElementAt(userCache.CurrentLine);
                Color ColorEmoji = _functions.EmojiToColor(CircleEmoji);

                CurrentLinee.ColorGuessColor[CurrentColumnPosition] = ColorEmoji;
                CurrentLinee.ColorGuessNumber[CurrentColumnPosition] = _functions.ColorToNumber(ColorEmoji);

                userCache.bitPicture = _functions.SingleDotColorEditor(userCache.ImageGraphics, userCache.bitPicture, userCache.CurrentLine, CurrentColumnPosition, ColorEmoji, userCache.DotsInWidth);

                userCache.DotColumnPostion += 1;
            } 

            
            _mMCache.RemoveCache(user.Id.ToString()); //just makes sure that there is something to cache
            _mMCache.StoreItems(userCache, user.Id.ToString());
            return userCache.bitPicture;
        }

        

        public Bitmap StartGame(int width, int height, IUser user, System.Drawing.Color colorIndicator)
        {
            MMUserCache userCache = new MMUserCache();
            userCache.Messages = new List<RestUserMessage>();
            userCache.DotIndicator = colorIndicator;
            userCache.CurrentGameRunning = true;
            userCache.DotsInHeight = height;
            userCache.DotsInWidth = width;
            userCache.DotColumnPostion = 0;
            userCache.SecretPattern = _functions.NumberPattern(width);

            Bitmap bitpicture = _functions.Emptyboard(height, width);
            userCache.ImageGraphics = Graphics.FromImage(bitpicture);
            userCache.bitPicture = _functions.AddGrayCircles(userCache.ImageGraphics, bitpicture, height, width, user.Username);
            userCache.Lines = new List<Line>();
            for (int i = 0; i < height; i++)
            {
                Line line = new Line();
                line.LineNum = i;
                line.ColorGuessColor = new List<Color>();
                line.ColorGuessNumber = new List<int>();
                for (int q = 0; q < width; q++)
                {
                    Color color = new Color();
                    int num = new int();
                    line.ColorGuessColor.Add(color);
                    line.ColorGuessNumber.Add(num);
                }
                
                userCache.Lines.Add(line);
            }
            if (_mMCache.CacheExists(user.Id.ToString())) _mMCache.RemoveCache(user.Id.ToString());
            _mMCache.StoreItems(userCache, user.Id.ToString());
            return userCache.bitPicture;
        }
        
        public bool IsSecondReactionAdded(ISocketMessageChannel socketMessage, SocketReaction socketReaction)
        {
            int PosOfMessage = -1;
            if (reactionMemories == null)
            {
                reactionMemories = new List<ReactionMemory>();
            }
            bool hasWork = false;
            foreach (var message in reactionMemories)
            {
                PosOfMessage++;
                if (message.socketMessages == socketMessage && message.socketReactions == socketReaction)
                {
                    hasWork =true;
                    break;
                    
                }
            }
            if (hasWork)
            {
                reactionMemories.RemoveAt(PosOfMessage);
                if (reactionMemories.Count > 40)
                {
                    for (int i = 0; i < reactionMemories.Count; i++)
                    {
                        reactionMemories.RemoveAt(i);
                    }
                }
                return true;
            }
            ReactionMemory reactionMemory = new ReactionMemory();
            reactionMemory.socketMessages = socketMessage;
            reactionMemory.socketReactions = socketReaction;
            reactionMemories.Add(reactionMemory);
            return false;
        }
        
        

        /*
        public bool IsSecondReactionAdded(ulong ReactionMessage, Emoji emoji, IUser user)
        {
            MMUserCache userCache;
            if (!_mMCache.CacheExists(user.Id.ToString()))
            {

                userCache = new MMUserCache();
            }
            else
            {
                userCache = _mMCache.RetrieveItem<MMUserCache>(user.Id.ToString());
            }
            int amountOfStoredmessages = userCache.Messages.Count;
            for (int i = 0; i < amountOfStoredmessages; i++)
            {
                if (userCache.Messages[i].Id == ReactionMessage)
                {
                    if (i == (amountOfStoredmessages - 1))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }

                }
            }
            return false;
        }
        */
        public bool ReactionOnRightMessage(ulong ReactionMessage, IUser user)
        {
            if (!_mMCache.CacheExists(user.Id.ToString()))
            {
                return false;
            }
            MMUserCache userCache = _mMCache.RetrieveItem<MMUserCache>(user.Id.ToString());
            if (!userCache.CurrentGameRunning)
            {
                return false;
            }
            int NumOfObjectsInlist = userCache.Messages.Count;
            if (ReactionMessage == userCache.Messages[NumOfObjectsInlist - 1].Id)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
  