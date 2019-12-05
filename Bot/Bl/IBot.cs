﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;
using Bot.Commands;
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
            Commands.RegistryCommand(new Command("addhomework","/addhomework текст дата(2019.11.26 - год,месяц, день)","Добавляет дз в базу","commandpermission.admin.addhomework"),new AddHomeWorkExecutor(helper,ErrorReporter));
            Commands.RegistryCommand(new Command("homework","/homework","Показывает домашнее задание на сегодня","commandpermission.defualt.homework"),new CheckHomeWorkExecutor(helper,ErrorReporter));
            
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