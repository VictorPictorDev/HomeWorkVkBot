using Bot.Bl;
using Bot.Commands;
using Bot.Commands.CustomCommands.HomeWorksCommands;
using Bot.Config;
using Bot.Homework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkNet.Model;

namespace Bot.Commands.CustomCommands.HomeWorksCommands
{
    public class DeleteHomeWorkExecutor : ICommandExecutor
    {
        public VkApiHelper Api;
        private HomeWorkHelper HomeWorkHelper;
        public HomeWorkExecutorHelper HomeWorkExecutorHelper;
        public DeleteHomeWorkExecutor(VkApiHelper helper, ErrorReporter reporter)
        {
            Api = helper;
            HomeWorkHelper = new HomeWorkHelper(reporter);
            HomeWorkExecutorHelper = new HomeWorkExecutorHelper(Api, reporter);
        }
        public bool Execute(BotUser sender, Command command, string Label, string[] parameters, Message VkMessage)
        {
            if (sender.HasPermission(command.Permission))
            {
                if (parameters.Length <= 0)
                {
                    return HomeWorkExecutorHelper.GetAllHomeWork(sender);
                }
                if (parameters.Length < 1)
                {
                    Api.SendMessage("Вы не вверли необходимый параметер(дату домашнего задания)", sender.UserId);
                    return false;
                }
                var datestr = parameters[0];
                var date = DateTime.ParseExact(datestr, Settings.Path.DateFormat, null);
                HomeWorkHelper.GetJsonItems();
                var res = HomeWorkHelper.GetJsonItemByDate(date);
                if (res == null)
                {
                    Api.SendMessage(ExecutorText.DeleteHomeWorkExecutor.ErrorDelete, sender.UserId);
                    return false;
                }
                HomeWorkHelper.Remove(res);
                HomeWorkHelper.UpdateJson();
                HomeWorkHelper.ClearData();
                Api.SendMessage(ExecutorText.DeleteHomeWorkExecutor.SuccessDelete, sender.UserId);
                return true;
            }
            Api.SendMessage(ExecutorText.CantPermission, sender.UserId);
            return false;
        }
    }
}
