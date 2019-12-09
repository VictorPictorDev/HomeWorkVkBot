using Bot.Bl;
using Bot.Config;
using Bot.Homework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;

namespace Bot.Commands.CustomCommands.HomeWorksCommands
{
    public class CheckHomeWorkExecutor:ICommandExecutor
    {
        public VkApiHelper Api;
        private HomeWorkHelper HomeWorkHelper;
        public HomeWorkExecutorHelper HomeWorkExecutorHelper;
        public CheckHomeWorkExecutor(VkApiHelper helper,ErrorReporter reporter)
        {
            Api = helper;
            HomeWorkHelper = new HomeWorkHelper(reporter);
            HomeWorkExecutorHelper = new HomeWorkExecutorHelper(Api, reporter);
        }
        private bool SendHomeWork(string datestr,BotUser sender)
        {
            try
            {
                HomeWorkHelper.GetHomeWorkList();
                if (HomeWorkHelper.HomeWorks == null)
                {
                    Api.SendMessage(ExecutorText.CheckHomeWorkExecutor.HomeWorkNull, sender.UserId);
                    return false;
                }
                var date = DateTime.ParseExact(datestr, Settings.Path.DateFormat, null);
                var res = HomeWorkHelper.GetHomeWork(date);
                if (res == null)
                {
                    Api.SendMessage(ExecutorText.CheckHomeWorkExecutor.HomeWorkDoesntAdded, sender.UserId);
                    return false;
                }
                Api.SendMessage($"Дз на {res.Date.DayOfWeek.ToString().ToUpper()}({res.Date.ToShortDateString()}): \n\n"+res.Text, sender.UserId);
                return true;
            }
            catch (Exception ex)
            {
                Api.SendMessage(ExecutorText.ExepctionText, sender.UserId);
                return false;
            }
        }
        public bool Execute(BotUser sender, Command command, string label, string[] parameters,Message VkMessage)
        {
            if (sender.HasPermission(command.Permission))
            {
                if (parameters.Length <= 0)
                {
                    return SendHomeWork(DateTime.Now.ToShortDateString(), sender);
                }

                if (parameters.Length == 1)
                {

                    if (parameters[0] == "all")
                    {
                        return HomeWorkExecutorHelper.GetAllHomeWork(sender);
                    }

                    return SendHomeWork(parameters[0],sender);
                }
            }
            Api.SendMessage(ExecutorText.CantPermission, sender.UserId);
            return false;
        }

    }
}
