using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using TastyBot.Services;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace TastyBot
{
    //See Startup.cs
    class Program
    {
        public static Task Main(string[] args)
            => Startup.RunAsync(args);
    }
}