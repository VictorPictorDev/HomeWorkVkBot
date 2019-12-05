using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot.Config;
namespace Bot.Database
{
    public class Database
    {
            private static Database instance;
            public SqlConnection Connection;
            private static object syncRoot = new Object();

            protected Database()
            {
                IDatabaseBuilder builder = new DatabaseBuilder();
                builder.SetDatasource(Settings.Db.DataSource);
                builder.SetAttachDbFileName(Settings.Db.AttachDbFileName);
                Connection = builder.Build();
            }

            public static Database getInstance()
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new Database();
                    }
                }
                return instance;
            }
    }
    public interface IDatabaseBuilder
    {
        void SetDatasource(string text);
        void SetDatabase(string text);
        void SetUsername(string text);
        void SetPassword(string text);
        void SetAttachDbFileName(string Text);
        SqlConnection Build();
        void Reset();
    }
    public class DatabaseBuilder:IDatabaseBuilder
    {
        SqlConnection resualt = new SqlConnection();
        string ConnStr;
        public DatabaseBuilder()
        {
            Reset();
        }
        public void SetDatasource(string text)
        {
            ConnStr = @"Data Source=" + text + ";";
        }

        public void SetDatabase(string text)
        {
            ConnStr = "Initial Catalog = " + text + ";";
        }

        public void SetUsername(string text)
        {
            ConnStr = "User ID = " + text + ";";
        }

        public void SetPassword(string text)
        {
            ConnStr = "Password = " + text +";";
        }

        public void Reset()
        {
            resualt = new SqlConnection();
        }

        public SqlConnection Build()
        {
            ConnStr += "Integrated Security=True;";
            SqlConnection temp = resualt;
            resualt.ConnectionString = ConnStr;
            Reset();
            return temp;
        }

        public void SetAttachDbFileName(string Text)
        {
            ConnStr = "AttachDbFilename = " + Text + ";";
        }
    }
}
