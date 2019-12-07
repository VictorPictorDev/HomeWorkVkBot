using Bot.Bl;
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
    public class AddHomeWorkExecutor : ICommandExecutor
    {
        private ExecutorText _text;
        public VkApiHelper Api { get; private set; }
        public HomeWorkHelper HomeWorkHelper;
        public HomeWorkExecutorHelper HomeWorkExecutorHelper;
        public AddHomeWorkExecutor(VkApiHelper helper,ErrorReporter errorReporter)
        {
            Api = helper;
            HomeWorkHelper = new HomeWorkHelper(errorReporter);
            _text = new ExecutorText();
            HomeWorkExecutorHelper = new HomeWorkExecutorHelper(Api, errorReporter);
        }

        
        public bool Execute(BotUser sender, Command command, string Label, string[] parameters,Message VkMessage)
        {
            if (sender.HasPermission(command.Permission))
            {
              
                if (parameters.Length<1)
                {
                    Api.SendMessage("Вы не вверли необходимый параметер(текст домашнего задания)",sender.UserId);
                    return false;
                }
                if (parameters.Length<2)
                {
                    Api.SendMessage("Вы не вверли необходимый параметер(дату домашнего задания)", sender.UserId);
                    return false;
                }
                var date = parameters[1];
                var text = parameters[0];
                var homeworkdate = DateTime.ParseExact(date, Settings.Path.DateFormat, null); 
                HomeWorkHelper.GetHomeWorkList();
                var h = HomeWorkHelper.GetHomeWork(homeworkdate);
                if (h != null)
                {
                    Api.SendMessage($"Вы не можете добавить домашнее задание на один и тот же день({homeworkdate.ToShortDateString()}). Для этого есть команда редактирования домашнего задания /edithomework", sender.UserId);
                    return false;
                }
                var homework = new HomeWork(text,sender.UserId,homeworkdate,VkMessage.Date.Value);
                return HomeWorkExecutorHelper.AddHomeWork(homework,sender);
            }
            
            Api.SendMessage(_text.CantPermission, sender.UserId);
            return false;
        }
    }
}
