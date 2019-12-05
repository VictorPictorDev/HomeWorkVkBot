using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot.Bl;
using Bot.Config;
using VkNet.Model.Attachments;
namespace Bot.Homework
{
    public class HomeWorkHelper
    {
        public List<HomeWork> HomeWorks;
        public ErrorReporter ErrorReporter;
        public HomeWorkHelper(ErrorReporter errorReporter)
        {
            HomeWorks = new List<HomeWork>();
            ErrorReporter = errorReporter;
        }
        public void GetHomeWorkList()
        {
            HomeWorks =  JsonHelper.Get<List<HomeWork>>(Settings.Path.HomeWorkPath);
        }
        public HomeWork GetHomeWork(DateTime date)
        {
            foreach(var h in HomeWorks)
            {
                if(h.Date.Day == date.Day&&h.Date.Month == date.Month&&h.Date.Year==date.Year)
                {
                    return h;
                }
            }
            return null;
        }
        public void AppendHomeWork(HomeWork homeWork)
        {
            if (HomeWorks.Count < 5)
            {
                HomeWorks.Add(homeWork);
            }
            else
            {
                ErrorReporter.Reporter(new ArgumentException("В дз не может быть больше 5 элементов"));
            }
        }
        public void RemoveHomeWork(HomeWork homeWork)
        {
            HomeWorks.Remove(homeWork);
        }
        public bool UpdateJson()
        {
            return JsonHelper.Set(Settings.Path.HomeWorkPath,HomeWorks);
        }
    }
}
