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

namespace Bot.Commands.CustomCommands
{
    public class AddHomeWorkExecutor : ICommandExecutor
    {
        private ExecutorText _text;
        public VkApiHelper Api { get; private set; }
        public HomeWorkHelper HomeWorkHelper; 
        
        public AddHomeWorkExecutor(VkApiHelper helper,ErrorReporter errorReporter)
        {
            Api = helper;
            HomeWorkHelper = new HomeWorkHelper(errorReporter);
            _text = new ExecutorText();
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
                try
                {
                    var homeworkdate = DateTime.ParseExact(parameters[1], Settings.Path.DateFormat, null);
                    if (homeworkdate.DayOfWeek == DayOfWeek.Saturday || homeworkdate.DayOfWeek == DayOfWeek.Sunday)
                    {
                        Api.SendMessage($"Вы не можете установить домашнее задание на выходные ({homeworkdate.ToShortDateString()}, {homeworkdate.DayOfWeek})", sender.UserId);
                        return false;
                    }
                    HomeWorkHelper.GetHomeWorkList();
                    var h = HomeWorkHelper.GetHomeWork(homeworkdate);
                    if (h != null)
                    {
                        Api.SendMessage($"Вы не можете добавить домашнее задание на один и тот же день({homeworkdate.ToShortDateString()}). Для этого есть команда редактирования домашнего задания", sender.UserId);
                        return false;
                    }
                    

                    var homework = new HomeWork(parameters[0], sender.UserId,homeworkdate, VkMessage.Date.Value);
                    HomeWorkHelper.AppendHomeWork(homework);
                    HomeWorkHelper.UpdateJson();
                    Api.SendMessage($"Домашнее задание успешно добавлено на дату {parameters[1]}", sender.UserId);
                    return true;
                }catch(Exception ex)
                {
                    Api.SendMessage("Что-то пошло не так! Проверьте правильность параметров команды.", sender.UserId);
                    return false;
                }
            }
            
            Api.SendMessage(_text.CantPermission, sender.UserId);
            return false;
        }
    }
}
