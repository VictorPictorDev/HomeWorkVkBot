using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkNet.Model.Attachments;

namespace Bot.Json
{
    public class JsonItem
    {
        public JsonItem(string Text, long AuthorId, DateTime date, DateTime postdate, List<Photo> attachment)
        {
            this.Text = Text;
            this.AuthorId = AuthorId;
            Date = date;
            PostDate = postdate;
            Attachments = attachment;
        }
        public string Text;
        public long AuthorId;
        public DateTime Date;
        public DateTime PostDate;
        public List<Photo> Attachments;
    }
}
