using Bot.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkNet.Model.Attachments;

namespace Bot.Shop
{
    public class ShopItem : JsonItem
    {
        public int Price;
        public static int Id_Counter;
        public int Id;
        public ShopItem(string Text, long AuthorId, DateTime date, DateTime postdate, List<Photo> attachment,int price) :base(Text, AuthorId, date, postdate, attachment)
        {
            Id_Counter += 1;
            Id = Id_Counter;
            Price = price;
        }
    }
}
