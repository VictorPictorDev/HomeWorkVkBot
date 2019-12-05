using System;
using System.Collections.Generic;
using System.Linq;
using VkNet;
using System.Text;
using System.Threading.Tasks;
using VkNet.Model.Attachments;
using System.Collections.ObjectModel;

namespace Bot.Homework
{
    public class HomeWork
    {
        public HomeWork(string Text,long AuthorId,DateTime date,DateTime postdate)
        {
            this.Text = Text;
            this.AuthorId = AuthorId;
            Date = date;
            PostDate = postdate;
        }
        public string Text;
        public long AuthorId;
        public DateTime Date;
        public DateTime PostDate;

    }
}
