using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot.Bl;
using Bot.Config;
using Bot.Homework;
using Bot.Json;
using VkNet.Model;
using VkNet.Model.Attachments;

namespace Bot.Commands.CustomCommands.HomeWorksCommands
{
    public class EditHomeWorkExecutor : ICommandExecutor
    {
        public VkApiHelper Api;
        private HomeWorkHelper HomeWorkHelper;
        public HomeWorkExecutorHelper HomeWorkExecutorHelper;
        public EditHomeWorkExecutor(VkApiHelper helper, ErrorReporter reporter)
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
                if (parameters.Length < 2)
                {
                    Api.SendMessage("Вы не вверли необходимый параметер(текст домашнего задания)", sender.UserId);
                    return false;
                }
                var datestr = parameters[0];
                var text = parameters[1];
                var date = DateTime.ParseExact(datestr, Settings.Path.DateFormat, null);
                HomeWorkHelper.GetJsonItems();
                var res = HomeWorkHelper.GetJsonItemByDate(date);
                if (res != null)
                {
                    HomeWorkHelper.Remove(res);
                    HomeWorkHelper.UpdateJson();
                }
                var list = new List<Photo>();
                foreach (var a in VkMessage.Attachments)
                {
                    if (a.Instance is Photo)
                    {
                        list.Add((Photo)a.Instance);
                    }
                }
                HomeWorkHelper.ClearData();
                var homework = new JsonItem(text, sender.UserId, date, VkMessage.Date.Value,list);
                return HomeWorkExecutorHelper.AddHomeWork(homework, sender);
            }
            Api.SendMessage(ExecutorText.CantPermission, sender.UserId);
            return false;
        }
    }
}
