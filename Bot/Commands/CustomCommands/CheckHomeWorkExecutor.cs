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

namespace Bot.Commands.CustomCommands
{
    public class CheckHomeWorkExecutor:ICommandExecutor
    {
        public VkApiHelper Api;
        private ExecutorText _text;
        private HomeWorkHelper HomeWorkHelper;
        public CheckHomeWorkExecutor(VkApiHelper helper,ErrorReporter reporter)
        {
            Api = helper;
            HomeWorkHelper = new HomeWorkHelper(reporter);
            
            _text = new ExecutorText();
        }
        private bool SendHomeWork(string datestr,BotUser sender)
        {
            try
            {
                HomeWorkHelper.GetHomeWorkList();
                var date = DateTime.ParseExact(datestr, Settings.Path.DateFormat, null);
                var res = HomeWorkHelper.GetHomeWork(date);
                if (res == null)
                {
                    Api.SendMessage($"Извините но дз на {date.ToShortDateString()} ещё не добавлено!", sender.UserId);
                    return false;
                }
                Api.SendMessage($"Дз на {res.Date.DayOfWeek.ToString().ToUpper()}({res.Date.ToShortDateString()}): \n\n"+res.Text, sender.UserId);
                return true;
            }
            catch (Exception ex)
            {
                Api.SendMessage("Что-то пошло не так! Проверьте правильность параметров команды.", sender.UserId);
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
                    return SendHomeWork(parameters[0],sender);
                }
            }
            Api.SendMessage(_text.CantPermission, sender.UserId);
            return false;
        }

    }
}
