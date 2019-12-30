using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot.Bl;
using Bot.Config;
using Bot.Json;
using VkNet.Model.Attachments;
namespace Bot.Homework
{
    public class HomeWorkHelper:IJsonItemHelper<JsonItem>
    {
        public static List<JsonItem> HomeWorks;
        public ErrorReporter ErrorReporter;
        public HomeWorkHelper(ErrorReporter errorReporter)
        {
            HomeWorks = new List<JsonItem>();
            ErrorReporter = errorReporter;
        }
        public void GetJsonItems()
        {
            
            var res = JsonHelper.Get<List<JsonItem>>(Settings.Path.HomeWorkPath);
            if (res != null)
            {
                HomeWorks = res;
            }
        }
        public JsonItem GetJsonItemByDate(DateTime date)
        {
            if (HomeWorks != null)
            {
                foreach (var h in HomeWorks)
                {
                    if (h.Date.Day == date.Day && h.Date.Month == date.Month && h.Date.Year == date.Year)
                    {
                        return h;
                    }
                }
            }
            return null;
        }
        public void Append(JsonItem homeWork)
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
        public void Remove(JsonItem homeWork)
        {
            HomeWorks.Remove(homeWork);
        }
        public bool UpdateJson()
        {
            return JsonHelper.Set(Settings.Path.HomeWorkPath,HomeWorks);
        }

        public void ClearData()
        {
            if (HomeWorks == null || HomeWorks.Count <= 0)
            {
                return;
            }
            HomeWorks.Clear();
        }
    }
}
