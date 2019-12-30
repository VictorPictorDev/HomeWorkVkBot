using Bot.Commands;
using Bot.Config;
using Bot.Shop;
using Bot.Bl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkNet.Model;
using VkNet.Model.Attachments;

namespace Bot.Commands.CustomCommands.ShopCommands
{
    public class ShopExecutor:ICommandExecutor
    {
        public VkApiHelper Api;
        public ShopHelper ShopHelper;
        public ShopExecutor(VkApiHelper helper)
        {
            Api = helper;
            ShopHelper = new ShopHelper();
        }
        public bool ShowAllShopItems(BotUser sender)
        {
            try
            {
                ShopHelper.GetJsonItems();
                if (ShopHelper.ShopItems == null||ShopHelper.ShopItems.Count<=0)
                {
                    Api.SendMessage("Извините, но в магазине нет неодного товара =(", sender.UserId);
                    return false;
                }
                string resualt = "Товары магазина: \n";
                foreach(var item in ShopHelper.ShopItems)
                {
                    resualt += item.Text + "\n";
                    resualt += $"Добавлено {item.PostDate.ToShortDateString()} \n";
                    resualt += $"Действительно до {item.Date.ToShortDateString()}\n";
                    resualt += "Цена: " + item.Price + "\n";
                    resualt += "\n";
                }
                
                Api.SendMessage(resualt,sender.UserId);
                return true;
            }
            catch (Exception ex)
            {
                Api.SendMessage(ExecutorText.ExepctionText, sender.UserId);
                return false;
            }
        }
        public bool Execute(BotUser sender, Command command, string label, string[] parameters, Message VkMessage)
        {
            if (sender.HasPermission(command.Permission))
            {
                return ShowAllShopItems(sender);
            }
            Api.SendMessage(ExecutorText.CantPermission, sender.UserId);
            return false;
        }
    }
}
