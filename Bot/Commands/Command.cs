using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Commands
{
    public class Command : ICommand
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Usage { get; private set; }
        public string Permission { get; private set; }
        public ICommandExecutor CommandExecutor { get; private set; }
        public Command(string Name, string Usage, string Description, string permission)
        {
            this.Name = Name;
            this.Usage = Usage;
            this.Description = Description;
            this.Permission = permission;
        }
        public void RegistryCommand(ICommandExecutor executor)
        {
            CommandExecutor = executor;
        }

        public void UnRegistryCommand(ICommandExecutor executor)
        {
            CommandExecutor = executor;
        }
    }
}
