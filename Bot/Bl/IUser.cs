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
        public bool IsSubscribe;
        public int Coins;
        public BotUser(long userid,int coins,bool issubscribe)
        {
            UserId = userid;
            IsSubscribe = issubscribe;
            Coins = coins;
        }
        public virtual User GetInfo(VkApi api,ProfileFields fields,NameCase nameCase)
        {
            var res = api.Users.Get(new long[] { UserId},fields,nameCase,true);
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
        public DefualtUser(long userid,int coins=0,bool IsSubscribe=false):base(userid,coins,IsSubscribe)
        {
            Permissions.Add("commandpermission.defualt.info");
            Permissions.Add("commandpermission.defualt.homework");
            Permissions.Add("commandpermission.defualt.profile");
            Permissions.Add("commandpermission.defualt.subscribe");
            Permissions.Add("commandpermission.defualt.unsubscribe");
            Permissions.Add("commandpermission.defualt.shop");
        }

        public override string ToString()
        {
            return this.GetType().Name;
        }
    }
    public class AdminUser : DefualtUser
    {
        public AdminUser(long userid, int coins = 0, bool IsSubscribe = false) :base(userid,coins,IsSubscribe)
        {
            Permissions.Add("commandpermission.admin.addhomework");
            Permissions.Add("commandpermission.admin.edithomework");
            Permissions.Add("commandpermission.admin.deletehomework");
            Permissions.Add("commandpermission.admin.shopitem");
        }
        public override string ToString()
        {
            return this.GetType().Name;
        }
    }
}
