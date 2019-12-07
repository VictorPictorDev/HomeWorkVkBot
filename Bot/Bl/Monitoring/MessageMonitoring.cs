using System;
using System.Linq;
using System.Threading;
using VkNet.Enums.SafetyEnums;
using VkNet.Model.RequestParams;
using VkNet.Model;
using Bot.Commands;

namespace Bot.Bl.Monitoring
{
    public interface IMonitoring
    {
        void Start();
        void Stop();

    }
    public class NewMessageEventArgs : EventArgs
    {
        public Message Message { get; private set; }
        public NewMessageEventArgs(Message message)
        {
            this.Message = message;
        }
    }

    public class CommandEventArgs : NewMessageEventArgs
    {
        public ICommand command { get; private set; }
        public string Label { get; private set; }
        public string[] parameters { get; private set; }
        public CommandEventArgs(Message message, ICommand command, string Label, string[] parameters) : base(message)
        {
            this.command = command;
            this.Label = Label;
            this.parameters = parameters;
        }
    }
    public class MessageMonitoring:IMonitoring
    {
        public Thread MonitoringThread { get; private set; }
        public VkApiHelper Helper { get; private set; }
        public delegate void NewMessageDelegate(object sender, NewMessageEventArgs args);
        public event NewMessageDelegate NewMessage;
        public delegate void CommandMessageDelegate(object sender,CommandEventArgs args);
        public event CommandMessageDelegate OnCommand;
        public event CommandMessageDelegate OnCommandNotFound;
        private ulong _groupId;
        public CommandList Commands { get; private set; }
        public MessageMonitoring(VkApiHelper helper,ulong groupId,CommandList commands)
        {
            Helper = helper;
            _groupId = groupId;
            Commands = commands;
            MonitoringThread = new Thread(new ThreadStart(MonitorMethod));
        }
        public void Start()
        {
            MonitoringThread.Start();
        }
        public void Stop()
        {
            MonitoringThread.Abort();
        }
        private void MonitorMethod()
        {
            while (true)
            {
                var Api =Helper.Api;
                var server = Api.Groups.GetLongPollServer(_groupId);
                var poll = Api.Groups.GetBotsLongPollHistory(new BotsLongPollHistoryParams()
                {
                    Server = server.Server,
                    Key = server.Key,
                    Ts = server.Ts,
                    Wait = 25
                });

                if (poll?.Updates == null) continue;
                foreach (var e in poll.Updates)
                {
                    if (e.Type == GroupUpdateType.MessageNew)
                    {
                        if (e.Message.Text.IndexOf("/") == 0)
                        {
                            var inputString = e.Message.Text.Trim().ToLower();
                            var StrCmdList = inputString.Split('"').Select((element, index) => index % 2 == 0 ? element.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries) : new string[] { element }).SelectMany(element => element).ToList();
                            var CmdName = StrCmdList[0];
                            StrCmdList.Remove(CmdName);
                            var res = Commands.GetCommand(CmdName.Remove(0,1));
                            if (res != null)
                            {
                                OnCommand?.Invoke(this, new CommandEventArgs(e.Message, res, inputString, StrCmdList.ToArray()));
                            }
                            else
                            {
                                OnCommandNotFound?.Invoke(this, new CommandEventArgs(e.Message, res, inputString, StrCmdList.ToArray()));
                                
                            }
                            break;
                        }
                        NewMessage?.Invoke(this, new NewMessageEventArgs(e.Message));
                        break;
                    }
                }
            }
        }
    }
}
