using Discord;
using Discord.Audio;
using Discord.Commands;
using MusicPlayer.Contracts;
using MusicPlayer.HelperClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


namespace DiscordUI.Modules
{
    /// <summary>
    /// The Module that handles all music player Things
    /// </summary>
    /*
    [Name("MusicPlayer")]
    public class MusicPlayerModule : ModuleBase<SocketCommandContext>
    {
        private readonly IMusicPlayerModule _musicPlayer;
        private readonly IVoiceChannel _channel;
        public MusicPlayerModule(IMusicPlayerModule musicPlayer)
        {
            _musicPlayer = musicPlayer;
        }
        
        [Command("play")]
        [Alias("p")]
        [Summary("Command used to play a song in the vc\t!p {youtube link}")]
        public async Task PlaySong(string Link = null)
        {
            if (Link == null)
            {
                await Context.Channel.SendMessageAsync($"<@{Context.User.Id}> Specify a link");
                return;
            }

            IVoiceChannel channel = (Context.User as IVoiceState).VoiceChannel;

            if (channel == null)
            {
                await Context.Channel.SendMessageAsync($"<@{Context.User.Id}> Must be in a vc to play a song");
                return;
            }
            

        }
    }
    */
}
