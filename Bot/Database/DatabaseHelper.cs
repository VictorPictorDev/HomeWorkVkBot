using Bot.Bl;
using Bot.Config;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
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
            Connection = DataBase.getInstance().Connection;
        }
        public bool IssetUser(long UserId)
        {
            var user = new DefualtUser(UserId);
            return IssetUser(user);
        }
        public BotUser GetUserById(long Id)
        {
            var query = new SqlCommand($"SELECT * FROM {Settings.Db.TableName} WHERE({Settings.Db.VkUserId})={Id} ", Connection);
            try
            {
                Connection.Open();
                using (SqlDataReader reader = query.ExecuteReader())
                {
                    if (reader.HasRows)
                    {

                        while (reader.Read())
                        {
                            long userId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("VkUserId")));
                            bool IsSubscribe = reader.GetBoolean(reader.GetOrdinal("IsSubscribe"));
                            int Coins = reader.GetInt32(reader.GetOrdinal("Coins"));
                            Type ourtype = typeof(BotUser); // Базовый тип
                            IEnumerable<Type> list = Assembly.GetAssembly(ourtype).GetTypes().Where(type => type.IsSubclassOf(ourtype));  // using System.Linq

                            foreach (Type itm in list)
                            {
                                if (itm.Name==reader.GetString(reader.GetOrdinal("Privilege")))
                                {
                                    ConstructorInfo ctor = itm.GetConstructor(new[] { typeof(long),typeof(int),typeof(bool) });
                                    BotUser obj = (BotUser)ctor.Invoke(new object[] { userId, Coins, IsSubscribe });
                                    return obj;
                                }
                            }
                        }
                    }
                    reader.Close();
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                Connection.Close();
            }
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
        public bool UpdateUser(BotUser user)
        {
            var sqlcommand = new SqlCommand($"UPDATE {Settings.Db.TableName} SET {Settings.Db.VkUserId}={user.UserId}, IsSubscribe={user.IsSubscribe}, Coins={user.Coins},Privilege={user} WHERE {Settings.Db.VkUserId}={user.UserId}", Connection);
            try
            {
                Connection.Open();
                sqlcommand.ExecuteNonQuery();
                return true;
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
            var sqlcommand = new SqlCommand($"INSERT INTO {Settings.Db.TableName} ({Settings.Db.VkUserId}, IsSubscribe, Coins,Privilege) VALUES (@UserId,@IsSubscribe,@Coins,@Privilege)", Connection);
            sqlcommand.Parameters.AddWithValue("@UserId", user.UserId);
            sqlcommand.Parameters.AddWithValue("@IsSubscribe", Convert.ToInt32(user.IsSubscribe));
            sqlcommand.Parameters.AddWithValue("@Coins", user.Coins);
            sqlcommand.Parameters.AddWithValue("@Privilege", user.ToString());
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
