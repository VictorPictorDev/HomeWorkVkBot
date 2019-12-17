using System;
using VkNet;
using VkNet.Model;
using Bot.Commands;
using Bot.Commands.CustomCommands.HomeWorksCommands;
using Bot.Commands.CustomCommands.UserExecutors;
using Bot.Commands.CustomCommands.SubscribeCommands;
using Bot.Bl.Monitoring;
using Bot.Commands.CustomCommands;
namespace Bot.Bl
{
    public interface IVkBot
    {
        bool Auth(string Token);
        bool Run();
        void Stop();
    }
    
    public class VkBot : IVkBot
    {
        public VkBotSettings Settings { get; private set; }
        public ErrorReporter ErrorReporter { get; private set; }
        public MessageMonitoring MessageMonitoring { get; private set; }
        public VkApiHelper helper { get; private set; } 
        public CommandList Commands { get; private set; }
        public VkBot(string token,Random random,ulong groupId,ErrorReporter reporter)
        {
            Commands = new CommandList();
            ErrorReporter = reporter;
            Settings = new VkBotSettings(new VkApi(),token,groupId);
            helper = new VkApiHelper(Settings.Api,random,ErrorReporter);
            MessageMonitoring = new MessageMonitoring(helper,groupId,Commands);

            InitCommand();
        }
        private void InitCommand()
        {
            Commands.RegistryCommand(new Command("info","/info","Выводит помощь по доступным вам командам","commandpermission.defualt.info"),new InfoExecutor(Commands.Commands,helper));
            Commands.RegistryCommand(new Command("addhomework","/addhomework <текст> <дата (26.11.2019 - год,месяц, день)>","Добавляет дз в базу","commandpermission.admin.addhomework"),new AddHomeWorkExecutor(helper,ErrorReporter));
            Commands.RegistryCommand(new Command("homework","/homework","Показывает домашнее задание на сегодня","commandpermission.defualt.homework"),new CheckHomeWorkExecutor(helper,ErrorReporter));
            Commands.RegistryCommand(new Command("edithomework", "/edithomework <дата (26.11.2019 - год,месяц, день)> <текст>", "Изменяет домашнее задание", "commandpermission.admin.edithomework"), new EditHomeWorkExecutor(helper, ErrorReporter));
            Commands.RegistryCommand(new Command("deletehomework", "/deletehomework <дата (26.11.2019 - год,месяц, день)>", "Удаляет домашнее задание", "commandpermission.admin.deletehomework"), new DeleteHomeWorkExecutor(helper, ErrorReporter));
            Commands.RegistryCommand(new Command("removehomework", "/removehomework <дата (26.11.2019 - год,месяц, день)>", "Удаляет домашнее задание", "commandpermission.admin.deletehomework"), new DeleteHomeWorkExecutor(helper, ErrorReporter));
            Commands.RegistryCommand(new Command("profile", "/profile", "Информация о вашем профиле", "commandpermission.defualt.profile"), new ProfileExecutor(helper));
        }
        public bool Auth(string Token)
        {
            try
            {
                Settings.Api.Authorize(new ApiAuthParams
                {
                    AccessToken = Token
                });
                return true;
            }
            catch (Exception ex)
            {
                ErrorReporter.ReportError(ex);
                return false;
            }
        }

        public bool Run()
        {
            if (!Auth(Settings.Token))
            {
                return false; 
            }
            MessageMonitoring.Start();
            return true;
        }

        public void Stop()
        {
            MessageMonitoring.Stop();
        }
    }
}
