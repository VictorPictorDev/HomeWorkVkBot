using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Json
{
    public interface IJsonItemHelper<T>
    {
        bool UpdateJson();
        void GetJsonItems();
        void Append(T item);
        void Remove(T item);
        void ClearData();
        T GetJsonItemByDate(DateTime date);
    }
}
