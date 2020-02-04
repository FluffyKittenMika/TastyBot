using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Microsoft.Extensions.Configuration;

namespace TastyBot.HpDungeon
{


    [Name("HpDungeon")]
    public class HpDungeonModule : ModuleBase<SocketCommandContext>
    {
        private readonly CommandService _service;
        private readonly IConfigurationRoot _config;

        /// <summary>
        /// Constructor, this one is called automagically through reflection
        /// </summary>
        /// <param name="service">Relevant service</param>
        /// <param name="config">Relevant config</param>
        public HpDungeonModule(CommandService service, IConfigurationRoot config)
        {
            _service = service;
            _config = config;
        }




    }
}
