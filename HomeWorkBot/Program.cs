using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot.Bl;
using Bot.Config;
using Bot.Commands;
using VkNet.Enums.SafetyEnums;
using VkNet.Model.Attachments;
using VkNet.Model.Keyboard;
using Bot.Bl.Monitoring;
using Bot.Database;
using VkNet.Enums.Filters;
using Bot.Homework;

namespace HomeWorkBot
{
    class Program
    {
        public static VkBot bot;
        static void Main(string[] args)
        {
            bot = new VkBot(Bot.Config.Settings.Bot.Token, new Random(), Bot.Config.Settings.Bot.GroupId, new ErrorReporter((ex) => { Console.WriteLine(ex.Message); }));
            bot.MessageMonitoring.NewMessage += MessageMonitoring_NewMessage;
            bot.MessageMonitoring.OnCommand += MessageMonitoring_OnCommand;
            bot.MessageMonitoring.OnCommandNotFound += MessageMonitoring_OnCommandNotFound;
            bot.Run();

            var monitoring = new DateTimeActionTimer(new DateTime(2019, 12, 9, Bot.Config.Settings.Path.DistTime[0], Bot.Config.Settings.Path.DistTime[1], 00), 30 * 1000, true);
            monitoring.Tick += Monitoring_Tick;
            monitoring.Start();
        }

        private static void Monitoring_Tick(object sender, EventArgs e)
        {
            var d = new Distribution(bot.helper);
            var homeworkhelper = new HomeWorkHelper(new ErrorReporter((ex) => { Console.WriteLine(ex.Message); }));

            homeworkhelper.GetJsonItems();
            var homework = homeworkhelper.GetJsonItemByDate(DateTime.Now);
            d.SendAllSubscribers(homework.Text);
            homeworkhelper.ClearData();
            
        }

        private static void MessageMonitoring_OnCommandNotFound(object sender, CommandEventArgs args)
        {
            var Sender = (MessageMonitoring)sender; 
            KeyboardBuilder key = new KeyboardBuilder();
            key.AddButton("/info", "", KeyboardButtonColor.Primary);
            MessageKeyboard keyboard = key.Build();
            Sender.Helper.SendMessage("Такой команды не существует напишите /info для помощи", args.Message.FromId.Value, keyboard);
        }

        private static void MessageMonitoring_OnCommand(object sender, CommandEventArgs args)
        {
            var bot = (MessageMonitoring)sender;
            var database = new DatabaseHelper(bot.Helper);
            BotUser user = new DefualtUser(args.Message.FromId.Value);
            if (!database.IssetUser(user).Result)
            {
                database.AddUser(user);
            }
            else
            {
                user = database.GetUserById(args.Message.FromId.Value).Result;
            }
            bot.Commands.Execute(user,(Command)args.command, args.Label, args.parameters,args.Message);
        }

        private static void MessageMonitoring_NewMessage(object sender, NewMessageEventArgs args)
        {
            var bot = (MessageMonitoring)sender;
            var database = new DatabaseHelper(bot.Helper);
            BotUser user = new DefualtUser(args.Message.FromId.Value);
            if (!database.IssetUser(user).Result)
            {
                database.AddUser(user);
            }
            else
            {
                user = database.GetUserById(args.Message.FromId.Value).Result;
            }
            bot.Helper.SendMessage($"Привет, акулёнок {user.GetInfo(bot.Helper.Api,ProfileFields.FirstName,NameCase.Nom).FirstName}. Данный бот создан для рассылки домашних заданий. В данном боте очень много комманд чтобы посмотреть все напиши /info.",user.UserId);
        }
    }
}
