using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Commands
{
    public interface ICommand
    {
        void RegistryCommand(ICommandExecutor executor);
        void UnRegistryCommand(ICommandExecutor executor);
    }
}
