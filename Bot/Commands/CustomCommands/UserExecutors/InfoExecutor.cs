using System;
using Bot.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot.Bl;
using Bot.Config;
using VkNet;
using VkNet.Model.Keyboard;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;

namespace Bot.Commands.CustomCommands.UserExecutors
{
    public class InfoExecutor : ICommandExecutor
    {
        public List<ICommand> Command { get; private set; }
        public VkApiHelper Api { get; private set; }
        public InfoExecutor(List<ICommand> Command,Bot.Bl.VkApiHelper helper)
        {
            this.Command = Command;
            Api = helper;
        }
        public bool Execute(BotUser sender, Command command, string label, string[] parameters,Message VkMessage)
        {
            if (sender.HasPermission(command.Permission))
            {
                if (Command == null)
                {
                    return false;
                }
                var resualt = ExecutorText.InfoExecutor.InfoDescription;
                foreach (Command cmd in Command)
                {
                    if (sender.HasPermission(cmd.Permission))
                    {
                        resualt += $"Команда /{cmd.Name} - {cmd.Description}.\n\n";
                    }
                }
                Api.SendMessage(resualt,sender.UserId);
                return true;
            }
            Api.SendMessage(ExecutorText.CantPermission,sender.UserId);
            return false;
        }
    }
}
