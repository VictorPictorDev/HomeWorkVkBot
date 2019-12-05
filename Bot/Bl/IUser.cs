using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;

namespace Bot.Bl
{
    public abstract class BotUser
    {
        public long UserId { get; private set; }
        public List<string> Permissions { get; private set; } = new List<string>();
        public BotUser(long userid)
        {
            UserId = userid;
        }
        public virtual User GetInfo(VkApi api,ProfileFields fields,NameCase nameCase)
        {
            var res = api.Users.Get(new long[] { UserId},fields,nameCase,false);
            if (res == null)
            {
                return null;
            }
            return res[0];
        }
        public virtual bool HasPermission(string Permission)
        {
            if (Permissions.Contains(Permission))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public class DefualtUser : BotUser
    {
        public DefualtUser(long userid):base(userid)
        {
            Permissions.Add("commandpermission.defualt.info");
            Permissions.Add("commandpermission.admin.addhomework");
            Permissions.Add("commandpermission.defualt.homework");
        }
    }
    public class AdminUser : DefualtUser
    {
        public AdminUser(long userid):base(userid)
        {
            Permissions.Add("");
        }
    }
}
