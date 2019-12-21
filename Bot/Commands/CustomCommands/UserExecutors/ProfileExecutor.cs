using Bot.Bl;
using Bot.Config;
using Bot.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkNet.Model;

namespace Bot.Commands.CustomCommands.UserExecutors
{
    public class ProfileExecutor:ICommandExecutor
    {
        public VkApiHelper Api { get; private set; }
        public ProfileExecutor(VkApiHelper helper)
        {
            Api = helper;
        }
        public bool Execute(BotUser sender, Command command, string label, string[] parameters, Message VkMessage)
        {
            if (sender.HasPermission(command.Permission))
            {
                string resualt = $"Ваш VkID: {sender.UserId} \n";
                resualt += $"Ваша привелегия: {sender.ToString()} \n";
                resualt += $"Баланс: {sender.Coins} эмигриун\n";
                resualt += $"Подписаны ли вы на рассылку домашних заданий: {sender.IsSubscribe}\n";
                Api.SendMessage(resualt, sender.UserId);
                return true;
            }
            Api.SendMessage(ExecutorText.CantPermission, sender.UserId);
            return false;
        }
    }
}
