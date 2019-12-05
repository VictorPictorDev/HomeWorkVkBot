using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot.Bl;
using VkNet.Model;

namespace Bot.Commands
{
    public class CommandList:ICommandList,ICommandExecutor
    {
        public List<ICommand> Commands { get; private set; } = new List<ICommand>();
        public Command GetCommand(string Name)
        {
            foreach (Command cmd in Commands)
            {
                if (cmd.Name == Name)
                {
                    return cmd;
                }
            }
            return null;
        }
        public bool Execute(BotUser sender, Command command, string Label, string[] parameters,Message VkMessage)
        {
            var cmd = GetCommand(command.Name);
            if (cmd == null || cmd.CommandExecutor == null)
            {
                return false;
            }
            return cmd.CommandExecutor.Execute(sender, command, Label, parameters,VkMessage);
        }

        public void RegistryCommand(ICommand command, ICommandExecutor executor)
        {
            if (command != null)
                command.RegistryCommand(executor);
            Commands.Add(command);
        }

        public void UnRegistryCommand(ICommand command)
        {
            if (Commands.Contains(command))
                Commands.Remove(command);
        }
    }
}
