using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Config
{
    public static class Settings
    {
        public static BotSettings Bot;
        public static DbSettings Db;
        public static PathSettings Path;
        static Settings()
        {
            Bot = new BotSettings() { Token = "2e4f6bd8da7dd033b7a7513fa64232e24d2302161b5642b8b627cfddaea1d317cdfdb7146ea31bb9d8478", GroupId= 132813846 };
            Db = new DbSettings() { TableName="Users",VkUserId="VkUserId",DataSource= @"(LocalDB)\MSSQLLocalDB", AttachDbFileName= @"D:\project\с#\Консольные проекты\HomeWorkBot\HomeWorkBot\Users.mdf" };
            Path = new PathSettings() {HomeWorkPath= "Json/HomeWorks.json", DateFormat= "dd.MM.yyyy",ShopItemsPath="Json/ShopItems.json",CoinName= "эмигриуна",DistTime = new int[] {18,00 } };
        }
    }
    public class BotSettings
    {
        public string Token;
        public ulong GroupId;

    }
    public class DbSettings
    {
        public string TableName;
        public string VkUserId;
        public string DataSource;
        public string AttachDbFileName;
    }
    public class PathSettings
    {
        public string HomeWorkPath;
        public string ShopItemsPath;
        public string DateFormat;
        public string CoinName;
        public int[] DistTime;
    }
}
