﻿using Bot.Bl;
using Bot.Config;
using Bot.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkNet.Model;

namespace Bot.Commands.CustomCommands.SubscribeCommands
{
    public class UnSubscribeExecutor : ICommandExecutor
    {
        public VkApiHelper Api { get; private set; }
        public UnSubscribeExecutor(VkApiHelper helper)
        {
            Api = helper;
        }
        public bool Execute(BotUser sender, Command command, string label, string[] parameters, Message VkMessage)
        {
            if (sender.HasPermission(command.Permission))
            {
            }

            Api.SendMessage(ExecutorText.CantPermission, sender.UserId);
            return false;
        }
    }
}