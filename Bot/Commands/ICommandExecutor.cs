using Bot.Bl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkNet.Model;

namespace Bot.Commands
{
    public interface ICommandExecutor
    {
        bool Execute(BotUser sender, Command command, string Label, string[] parameters,Message VkMessage);
    }
}
