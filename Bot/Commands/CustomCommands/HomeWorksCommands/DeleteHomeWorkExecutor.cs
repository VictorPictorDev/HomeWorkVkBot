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
        private ExecutorText _text;
        private HomeWorkHelper HomeWorkHelper;
        public HomeWorkExecutorHelper HomeWorkExecutorHelper;
        public DeleteHomeWorkExecutor(VkApiHelper helper, ErrorReporter reporter)
        {
            Api = helper;
            HomeWorkHelper = new HomeWorkHelper(reporter);

            _text = new ExecutorText();
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
                HomeWorkHelper.GetHomeWorkList();
                var res = HomeWorkHelper.GetHomeWork(date);
                if (res != null)
                {
                    HomeWorkHelper.RemoveHomeWork(res);
                    HomeWorkHelper.UpdateJson();
                }
                return true;
            }
            Api.SendMessage(_text.CantPermission, sender.UserId);
            return false;
        }
    }
}
