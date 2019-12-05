using Bot.Bl;
using Bot.Config;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Database
{
    public class DatabaseHelper
    {
        SqlConnection Connection;
        public VkApiHelper Helper;
        public DatabaseHelper(VkApiHelper helper)
        {
            Helper = helper;
            Connection = Database.getInstance().Connection;
        }
        public bool IssetUser(BotUser user)
        {
            var query = new SqlCommand($"SELECT COUNT(*) FROM {Settings.Db.TableName} WHERE({Settings.Db.VkUserId})={ user.UserId} ",Connection);
            try
            {
                Connection.Open();
                var res = (int)query.ExecuteScalar();
                if(res == default(int))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Helper.SendMessage(ex.Message, user.UserId);
                return false;
            }
            finally
            {
                Connection.Close();
            }
        }
        public bool AddUser(BotUser user)
        {

            if (IssetUser(user))
            {
                return false;
            }
            var sqlcommand = new SqlCommand($"INSERT INTO {Settings.Db.TableName} ({Settings.Db.VkUserId}) VALUES (@{Settings.Db.VkUserId})", Connection);
            try
            {
                Connection.Open();
                sqlcommand.ExecuteNonQuery();
                return true;
            }
            catch(Exception ex)
            {
                Helper.SendMessage(ex.Message, user.UserId);
                return false;
            }
            finally
            {
                Connection.Close();
            }
        }
        public bool RemoveUser(BotUser user)
        {
            var removeuser = new SqlCommand($"DELETE FROM {Settings.Db.TableName} WHERE {Settings.Db.VkUserId}={user.UserId}", Connection);
            try
            {
                Connection.Open();
                int res = removeuser.ExecuteNonQuery();
                if (res == default(int))
                {
                    
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Helper.SendMessage(ex.Message, user.UserId);
                return false;
            }
            finally
            {
                Connection.Close();
            }
        }
    }
}
