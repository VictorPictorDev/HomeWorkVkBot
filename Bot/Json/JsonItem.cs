using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Json
{
    public class JsonItem
    {
        public JsonItem(string Text, long AuthorId, DateTime date, DateTime postdate)
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
