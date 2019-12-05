using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkNet;
using VkNet.Model.Attachments;
using VkNet.Model.Keyboard;
using VkNet.Model.RequestParams;

namespace Bot.Bl
{
    public class VkApiHelper
    {
        public VkApi Api { get; private set; }
        public IErrorReporter Reporter { get; private set; }
        public Random Random { get; private set; }
        public VkApiHelper(VkApi api,Random rand,IErrorReporter reporter)
        {
            Api = api;
            Random = rand;
            Reporter = reporter;
        }
        public bool SendMessage(string Message, long userId)
        {
            try
            {
                Api.Messages.Send(new MessagesSendParams()
                {
                    UserId = userId,
                    Message = Message,
                    RandomId = Random.Next()

                });
                return true;
            }
            catch (Exception ex)
            {
                Reporter.ReportError(ex);
                return false;
            }
        }
        public bool SendMessage(string Message, long userId,MessageKeyboard keyboard)
        {
            try
            {
                Api.Messages.Send(new MessagesSendParams()
                {
                    UserId = userId,
                    Message = Message,
                    RandomId = Random.Next(),
                    Keyboard = keyboard

                });
                return true;
            }
            catch (Exception ex)
            {
                Reporter.ReportError(ex);
                return false;
            }
        }
        public bool SendMessage(string Message, long userId,List<MediaAttachment> attachments)
        {
            try
            {
                Api.Messages.Send(new MessagesSendParams()
                {
                    UserId = userId,
                    Message = Message,
                    RandomId = Random.Next(),
                    Attachments = attachments 
                });
                return true;
            }
            catch (Exception ex)
            {
                Reporter.ReportError(ex);
                return false;
            }
        }

    }
}
