using Bot.Config;
using Bot.Homework;
using System;
using Bot.Bl;
using Bot.Json;

namespace Bot.Commands.CustomCommands.HomeWorksCommands
{
    public class HomeWorkExecutorHelper
    {
        public VkApiHelper Api;
        public HomeWorkHelper HomeWorkHelper;
        public HomeWorkExecutorHelper(VkApiHelper helper, ErrorReporter reporter)
        {
            Api = helper;
            HomeWorkHelper = new HomeWorkHelper(reporter);
        }
        public bool AddHomeWork(JsonItem homework,BotUser sender)
        {
            try
            {
                
                if (homework.Date.DayOfWeek == DayOfWeek.Saturday || homework.Date.DayOfWeek == DayOfWeek.Sunday)
                {
                    Api.SendMessage($"Вы не можете установить домашнее задание на выходные {homework.Date.ToShortDateString()}, {homework.Date.DayOfWeek})", sender.UserId);
                    return false;
                }
                HomeWorkHelper.GetJsonItems();
                HomeWorkHelper.Append(homework);
                HomeWorkHelper.UpdateJson();
                Api.SendMessage($"Домашнее задание успешно добавлено на дату {homework.Date.ToShortDateString()} ", sender.UserId);
                return true;
            }
            catch (Exception ex)
            {
                Api.SendMessage(ExecutorText.ExepctionText, sender.UserId);
                return false;
            }
        }
        public bool GetAllHomeWork(BotUser sender)
        {
            HomeWorkHelper.GetJsonItems();
            if (HomeWorkHelper.HomeWorks == null)
            {
                Api.SendMessage("Список домашних заданий пуст", sender.UserId);
                return false;
            }
            string resualt = "Список всех домашних заданий:\n\n";
            for (int i = 0; i < HomeWorkHelper.HomeWorks.Count; i++)
            {
                var h = HomeWorkHelper.HomeWorks[i];
                var shortText = string.Empty;
                if (h.Text.Length > 10)
                {
                    shortText = h.Text.Substring(0, 10);
                }
                else
                {
                    shortText = h.Text;
                }
                resualt+=$"Номер [{i}] - {shortText}... , дата {h.PostDate.ToShortDateString()}\n";
            }
            Api.SendMessage(resualt, sender.UserId);
            return true;
        }
    }
}
