using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot.Bl;
using Bot.Config;
using Bot.Homework;
using VkNet.Model;

namespace Bot.Commands.CustomCommands.HomeWorksCommands
{
    public class EditHomeWorkExecutor : ICommandExecutor
    {
        public VkApiHelper Api;
        private ExecutorText _text;
        private HomeWorkHelper HomeWorkHelper;
        public HomeWorkExecutorHelper HomeWorkExecutorHelper;
        public EditHomeWorkExecutor(VkApiHelper helper, ErrorReporter reporter)
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
                if (parameters.Length < 2)
                {
                    Api.SendMessage("Вы не вверли необходимый параметер(текст домашнего задания)", sender.UserId);
                    return false;
                }
                var datestr = parameters[0];
                var text = parameters[1];
                var date = DateTime.ParseExact(datestr, Settings.Path.DateFormat, null);
                HomeWorkHelper.GetHomeWorkList();
                var res = HomeWorkHelper.GetHomeWork(date);
                if (res != null)
                {
                    HomeWorkHelper.RemoveHomeWork(res);
                    HomeWorkHelper.UpdateJson();
                }
                var homework = new HomeWork(text, sender.UserId, date, VkMessage.Date.Value);
                return HomeWorkExecutorHelper.AddHomeWork(homework, sender);
            }
            Api.SendMessage(_text.CantPermission, sender.UserId);
            return false;
        }
    }
}
