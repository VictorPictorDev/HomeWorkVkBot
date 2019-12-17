using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Json
{
    public interface IJsonItemHelper
    {
        bool UpdateJson();
        void GetJsonItems();
        void Append(JsonItem item);
        void Remove(JsonItem item);
        JsonItem GetJsonItemByDate(DateTime date);
    }
}
