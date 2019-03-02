using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class DalService
    {
        private static DalService INSTANCE;
        private string connetionString = "Server= localhost; Database= DemoDB; Integrated Security=True;";
        private SqlConnection sqlConn;
        private string sqlCommandString;
        private SqlCommand sqlCommand;

        private DalService()
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

        public bool testLogin(string user, string password)
        {
            sqlConn.Open();
            sqlCommandString = "Select * from Users";
            sqlCommand = new SqlCommand(sqlCommandString, sqlConn);
            using (var reader = sqlCommand.ExecuteReader())
            {
                while (reader.Read())
                {

                    string userFromSql = reader["UserName"].ToString();
                    string passwordFromSql= reader["Password"].ToString();
                    if(userFromSql.Equals(user) && passwordFromSql.Equals(password))
                    {
                        sqlConn.Close();
                        return true;
                    }

                }
            }

            sqlConn.Close();
            return false;
        }

    }
}
