using Bot.Config;
using Bot.Database;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Bl.Monitoring
{
    public class Distribution
    {
        public VkApiHelper ApiHelper;
        
        public Distribution(VkApiHelper helper)
        {
            ApiHelper = helper;

        }
        public bool Send(string Text,List<BotUser> users)
        {
            foreach (var user in users)
            {
                if (!ApiHelper.SendMessage(Text, user.UserId))
                {
                    return false;
                }
            }
            return false;
        }
        public bool SendAllSubscribers(string Text)
        {
            var Connection = DataBase.getInstance().Connection;
            var query = new SqlCommand($"SELECT {Settings.Db.VkUserId} FROM {Settings.Db.TableName}", Connection);
            try
            {
                Connection.Open();
                using (SqlDataReader reader = query.ExecuteReader())
                {
                    if (reader.HasRows)
                    {

                        while (reader.Read())
                        {
                            long userId = Convert.ToInt32(reader.GetValue(0));
                            bool issubscribe = reader.GetBoolean(2);
                            if (issubscribe)
                            {
                                ApiHelper.SendMessage(Text, userId);
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                    reader.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                Connection.Close();
            }
        }
        public bool SendAll(string Text)
        {
            var Connection = DataBase.getInstance().Connection;
            var query = new SqlCommand($"SELECT {Settings.Db.VkUserId} FROM {Settings.Db.TableName}", Connection);
            try
            {
                Connection.Open();
                using (SqlDataReader reader = query.ExecuteReader())
                {
                    if (reader.HasRows)
                    {

                        while (reader.Read())
                        {
                            long userId = Convert.ToInt32(reader.GetValue(0));
                            ApiHelper.SendMessage(Text,userId);
                        }
                    }
                    reader.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                Connection.Close();
            }
        }
    }
}
