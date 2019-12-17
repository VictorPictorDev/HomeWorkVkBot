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

namespace HomeWorkBot
{
    class Program
    {
        static void Main(string[] args)
        {
            var bot = new VkBot(Settings.Bot.Token, new Random(), Settings.Bot.GroupId, new ErrorReporter((ex) => { Console.WriteLine(ex.Message); }));
            bot.MessageMonitoring.NewMessage += MessageMonitoring_NewMessage;
            bot.MessageMonitoring.OnCommand += MessageMonitoring_OnCommand;
            bot.MessageMonitoring.OnCommandNotFound += MessageMonitoring_OnCommandNotFound;
            bot.Run();
            //var monitoring = new DateTimeActionTimer(new DateTime(2019,12,9,17,04,10),60*1000,false);
            //monitoring.Tick += Monitoring_Tick;
            //monitoring.Start();
        }

        private static void Monitoring_Tick(object sender, EventArgs e)
        {
            Console.WriteLine("IT`S WORK!");
        }

        private static void MessageMonitoring_OnCommandNotFound(object sender, CommandEventArgs args)
        {
            var Sender = (MessageMonitoring)sender; 
            KeyboardBuilder key = new KeyboardBuilder();
            key.AddButton("/info", "хз что это", KeyboardButtonColor.Primary);
            MessageKeyboard keyboard = key.Build();
            Sender.Helper.SendMessage("Такой команды не существует напишите /info для помощи", args.Message.FromId.Value, keyboard);
        }

        private static void MessageMonitoring_OnCommand(object sender, CommandEventArgs args)
        {
            var bot = (MessageMonitoring)sender;
            var database = new DatabaseHelper(bot.Helper);
            BotUser user = new DefualtUser(args.Message.FromId.Value);
            if (!database.IssetUser(user))
            {
                database.AddUser(user);
            }
            else
            {
                user = database.GetUserById(args.Message.FromId.Value);
            }
            bot.Commands.Execute(user,(Command)args.command, args.Label, args.parameters,args.Message);
        }

        private static void MessageMonitoring_NewMessage(object sender, NewMessageEventArgs args)
        {
            var bot = (MessageMonitoring)sender;
            var database = new DatabaseHelper(bot.Helper);
            BotUser user = new DefualtUser(args.Message.FromId.Value);
            if (!database.IssetUser(user))
            {
                database.AddUser(user);
            }
            else
            {
                user = database.GetUserById(args.Message.FromId.Value);
            }
            bot.Helper.SendMessage("Привет, друг",user.UserId);
        }
    }
}
