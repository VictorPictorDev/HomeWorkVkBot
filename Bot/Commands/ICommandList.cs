using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Commands
{
    public interface ICommandList
    {
        void RegistryCommand(ICommand command, ICommandExecutor executor);
        void UnRegistryCommand(ICommand command);
    }
}
