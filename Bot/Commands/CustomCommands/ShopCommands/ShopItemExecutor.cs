using Bot.Bl;
using Bot.Commands;
using Bot.Config;
using Bot.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkNet.Model;
using VkNet.Model.Attachments;

namespace Bot.Commands.CustomCommands.ShopCommands
{
    public class ShopItemExecutor : ICommandExecutor
    {
        public VkApiHelper Api;
        public ShopHelper ShopHelper;
        public ShopItemExecutor(VkApiHelper helper)
        {
            Api = helper;
            ShopHelper = new ShopHelper();
        }
        public ShopItem GetShopItemByStringValue(string Text, long AuthorId, string Date,DateTime PostDate,List<Photo> Attachment,string Price)
        {
            DateTime converted_date;
            int converted_price;
            try
            {
                converted_date = DateTime.ParseExact(Date, Settings.Path.DateFormat, null);
                converted_price = Convert.ToInt32(Price);
            }
            catch (Exception ex)
            {
                return null;
            }
            return new ShopItem(Text,AuthorId,converted_date,PostDate,Attachment,converted_price);
        }
        public bool CheckParametersOnValid(string[] parameters, BotUser sender,params string[] args)
        {
            if (parameters.Length <= 0||parameters.Length>args.Length)
            {
                return false;
            }
            for(int i = 0; i < args.Length; i++)
            {
                if (parameters.Length <= i)
                {
                    Api.SendMessage("Вы не ввели необходимый параметер "+args[i], sender.UserId);
                    return false;
                }
            }
            return true;
        }
        public bool ShowAllShopItems(BotUser sender)
        {
            ShopHelper.GetJsonItems();
            if (ShopHelper.ShopItems == null || ShopHelper.ShopItems.Count <= 0)
            {
                Api.SendMessage("Список магазина пуст", sender.UserId);
                return false;
            }
            string resualt = "Список всех предметов магазина:\n\n";
            for (int i = 0; i < ShopHelper.ShopItems.Count; i++)
            {
                var h = ShopHelper.ShopItems[i];
                var shortText = string.Empty;
                if (h.Text.Length > 15)
                {
                    shortText = h.Text.Substring(0, 15) + "...";
                }
                else
                {
                    shortText = h.Text;
                }
                resualt += $"ID товара [{h.Id}] - {shortText}, дата публикации {h.PostDate.ToShortDateString()}, дата окончания {h.Date.ToShortDateString()},цена {h.Price}\n\n";
            }
            ShopHelper.ClearData();
            Api.SendMessage(resualt, sender.UserId);
            return true;
        }
        public bool DeleteShopItem(string Id,BotUser sender)
        {
            try
            {
                var converted_id = Convert.ToInt32(Id);
                ShopHelper.GetJsonItems();
                var resualt = ShopHelper.GetShopItemById(converted_id);
                if (resualt == null)
                {
                    Api.SendMessage("Вы не можете удалить не существующий товар!", sender.UserId);
                    return false;
                }
                ShopHelper.Remove(resualt);
                ShopHelper.UpdateJson();
                ShopHelper.ClearData();
                Api.SendMessage("Вы успешно удалили товар!", sender.UserId);
                return true;
            }
            catch (Exception ex)
            {
                Api.SendMessage("Вы ввели некоректный ID", sender.UserId);
                return false;
            }
        }

        public bool EditShopItem(string id,string text,string price,string date,BotUser sender,Message VkMessage)
        {

            var list = new List<Photo>();
            foreach (var a in VkMessage.Attachments)
            {
                if (a.Instance is Photo)
                {
                    list.Add((Photo)a.Instance);
                }
                else
                {
                    Api.SendMessage("[ОШИБКА]: Вы не можете добавить предмет в магазин, так как вложение не фото.", sender.UserId);
                    return false;
                }
            }

            try
            {
                ShopHelper.GetJsonItems();
                var GetingShopItem = ShopHelper.GetShopItemById(Convert.ToInt32(id));
                if (GetingShopItem == null)
                {
                    Api.SendMessage("Товара по данному ID не было найдено! Возможно данного товара с таким ID не существует или вы ввели неправельный ID", sender.UserId);
                    return false;
                }
                GetingShopItem.Text = text ?? GetingShopItem.Text;
                if(price != null)GetingShopItem.Price = Convert.ToInt32(price);
                if(date!=null)GetingShopItem.Date = DateTime.ParseExact(date, Settings.Path.DateFormat, null);
                if(list!=null||list.Count>0)GetingShopItem.Attachments = list;
                GetingShopItem.PostDate = VkMessage.Date.Value;
                GetingShopItem.AuthorId = sender.UserId;
                ShopHelper.UpdateJson();
                ShopHelper.ClearData();
                Api.SendMessage("Вы успешно изменили товар!", sender.UserId);
                return true;
            }
            catch (Exception ex)
            {
                Api.SendMessage("Вы ввели некоректный ID товара!", sender.UserId);
                return false;
            }
        }

        public bool Execute(BotUser sender, Command command, string Label, string[] parameters, Message VkMessage)
        {
            
            if (sender.HasPermission(command.Permission))
            {
                if (parameters.Length <= 0)
                {
                    Api.SendMessage("Вы не ввели не одну подкоманду."+command.Usage, sender.UserId);
                    return false;
                }

                if (parameters.Length >= 1)
                {
                    var first = parameters[0];
                    switch (first)
                    {
                        case "add":
                            if(CheckParametersOnValid(parameters, sender, "add" ,"text", "date", "price"))
                            {
                                var text = parameters[1];
                                var date = parameters[2];
                                var price = parameters[3];

                                var list = new List<Photo>();
                                if (VkMessage.Attachments == null || VkMessage.Attachments.Count <= 0)
                                {
                                    Api.SendMessage("[ОШИБКА]: Вы не можете добавить предмет в магазин без фото.", sender.UserId);
                                    return false;
                                }
                                foreach (var a in VkMessage.Attachments)
                                {
                                    if (a.Instance is Photo)
                                    {
                                        list.Add((Photo)a.Instance);
                                    }
                                    else
                                    {
                                        Api.SendMessage("[ОШИБКА]: Вы не можете добавить предмет в магазин, так как вложение не фото.", sender.UserId);
                                        return false;
                                    }
                                }
                                ShopHelper.GetJsonItems();
                                var shopitem = GetShopItemByStringValue(text, sender.UserId, date, VkMessage.Date.Value,list,price);
                                if (shopitem == null)
                                {
                                    Api.SendMessage("[ОШИБКА]: Вы ввели некоректный параметер или допустили ошибку в параметре. (Пример некоректного ввода: 30 000 монет - некоректно, 30000 - коректно)", sender.UserId);
                                    return false;
                                }
                                ShopHelper.Append(shopitem);
                                ShopHelper.UpdateJson();
                                ShopHelper.ClearData();
                                Api.SendMessage("Предмет успешно добавлен в магазин!", sender.UserId);
                                return true;
                            }
                            else
                            {
                                return false;
                            }

                            break;
                        case "edit":
                            if (parameters.Length <= 1)
                            {
                                return ShowAllShopItems(sender);
                            }
                            if (CheckParametersOnValid(parameters, sender, "edit", "id", "text"))
                            {
                                var id = parameters[1];
                                var text = parameters[2];
                                return EditShopItem(id, text, null, null, sender, VkMessage);
                            }
                            if (CheckParametersOnValid(parameters, sender, "edit", "id", "text", "date"))
                            {
                                var id = parameters[1];
                                var text = parameters[2];
                                var date = parameters[3];
                                return EditShopItem(id, text, null, date, sender, VkMessage);
                            }
                            if (CheckParametersOnValid(parameters, sender, "edit", "id", "text", "date","price"))
                            {
                                var id = parameters[1];
                                var text = parameters[2];
                                var date = parameters[3];
                                var price = parameters[4];
                                return EditShopItem(id,text,price,date,sender,VkMessage);
                            }
                            break;
                        case "delete":
                            if(parameters.Length<=1)
                            {
                                return ShowAllShopItems(sender);
                            }
                            if(CheckParametersOnValid(parameters, sender, "delete","id"))
                            {
                                var id = parameters[1];
                                return DeleteShopItem(id,sender);
                            }
                            break;
                        default:
                            Api.SendMessage("Данной подкоманды не существует."+command.Usage, sender.UserId);
                            return false;
                            break;
                    }
                }
            }
            Api.SendMessage(ExecutorText.CantPermission, sender.UserId);
            return false;
        }
    }
}
