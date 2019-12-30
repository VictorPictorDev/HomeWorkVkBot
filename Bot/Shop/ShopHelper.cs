using Bot.Bl;
using Bot.Config;
using Bot.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Shop
{
    public class ShopHelper : IJsonItemHelper<ShopItem>
    {
        public static List<ShopItem> ShopItems;
        public ShopHelper()
        {
            ShopItems = new List<ShopItem>();
        }
        public void GetJsonItems()
        {

            var res = JsonHelper.Get<List<ShopItem>>(Settings.Path.ShopItemsPath);
            if (res != null)
            {
                ShopItems = res;
                ShopItem.Id_Counter = ShopItems.LastOrDefault().Id;
            }
        }
        public ShopItem GetJsonItemByDate(DateTime date)
        {
            if (ShopItems != null)
            {
                foreach (var h in ShopItems)
                {
                    if (h.Date.Day == date.Day && h.Date.Month == date.Month && h.Date.Year == date.Year)
                    {
                        return h;
                    }
                }
            }
            return null;
        }
        public ShopItem GetShopItemById(int Id)
        {
            if (ShopItems != null)
            {
                foreach (var h in ShopItems)
                {
                    if (h.Id==Id)
                    {
                        return h;
                    }
                }
            }
            return null;
        }
        public void Append(ShopItem homeWork)
        {
            ShopItems.Add(homeWork);
            
        }
        public void Remove(ShopItem homeWork)
        {
            ShopItems.Remove(homeWork);
        }
        public bool UpdateJson()
        {
            return JsonHelper.Set(Settings.Path.ShopItemsPath, ShopItems);
        }

        public void ClearData()
        {
            if (ShopItems == null || ShopItems.Count <= 0)
            {
                return;
            }
            ShopItems.Clear();
        }
    }
}
