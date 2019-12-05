using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkNet;

namespace Bot.Bl
{
    public struct VkBotSettings
    {
        public string Token { get; private set; }
        public ulong GroupId { get; private set; }
        public VkApi Api { get; private set; }
        public VkBotSettings(VkApi api,string _token,ulong _groupId)
        {
            this.Api = api;
            Token = _token;
            GroupId = _groupId;
        }
    }
}
