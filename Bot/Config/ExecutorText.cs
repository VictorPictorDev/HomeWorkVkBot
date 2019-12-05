using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Config
{
    public class ExecutorText
    {
        public InfoExecutorText InfoExecutor { get; private set; } = new InfoExecutorText();
        public string CantPermission = "Вы не имеете права на использование данной команды!";
    }
    public class InfoExecutorText
    {
        public string InfoDescription = "Список доступных команд:\n\n";
    }
    public class DefualtExecotorText
    {
    }
    public class SusseccExecutorText
    {
    }
    public class ErrorExecutorText
    {
    }
}
