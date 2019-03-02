using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TorrentLibrary
{
    public class DalService
    {
        private static DalService INSTANCE;
        private string connetionString = "Server= localhost; Database= DemoDB; Integrated Security=True;";
        private SqlConnection sqlConn;
        private string sqlCommandString;
        private SqlCommand sqlCommand;

        private const string USERNAME_COLUMN = "UserName";
        private const string PASSWORD_COLUMN = "Password";
        private const string USERS_TABLE = "Users";

        public DalService()
        {
            sqlConn = new SqlConnection(connetionString);
        }

        public static DalService getInstance()
        {
            if (DalService.INSTANCE == null)
            {
                INSTANCE = new DalService();
            }
            return INSTANCE;
        }

        public bool stupidLogin(string user, string password)
        {
            sqlConn.Open();
            sqlCommandString = "Select * from Users";
            sqlCommand = new SqlCommand(sqlCommandString, sqlConn);
            using (var reader = sqlCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    string userFromSql = reader[USERNAME_COLUMN].ToString();
                    string passwordFromSql = reader[PASSWORD_COLUMN].ToString();
                    if (userFromSql.Equals(user) && passwordFromSql.Equals(password))
                    {
                        sqlConn.Close();
                        return true;
                    }

                }
            }

            sqlConn.Close();
            return false;
        }

        public bool login(string user, string password)
        {
            sqlConn.Open();
            sqlCommandString = "Select * from " + USERS_TABLE + " where " + USERNAME_COLUMN + "='" + user + "' and " + PASSWORD_COLUMN + "='" + password + "'";
            sqlCommand = new SqlCommand(sqlCommandString, sqlConn);

            var reader = sqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
                sqlConn.Close();
                return true;
            }
            sqlConn.Close();
            return false;
        }

        public bool register(string user, string password)
        {
            sqlConn.Open();
            sqlCommandString = "insert into " + USERS_TABLE + " VALUES('" + user + "','" + password + "');";
            sqlCommand = new SqlCommand(sqlCommandString, sqlConn);
            try
            {
                sqlCommand.ExecuteReader();
            }catch(Exception e)
            {
                sqlConn.Close();
                Console.WriteLine("User already exists");
                return false;
            }
            
            sqlConn.Close();
            return true;
        }
    }
}
