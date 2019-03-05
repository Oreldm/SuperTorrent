using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace ClassLibrary
{
    public class DAL
    {
        private static DAL ins;

        const string connectionString = "Data Source=(localdb)\\ProjectsV13;Initial Catalog=Torrent;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;MultipleActiveResultSets=true";//MultipleActiveResultSets=true allows multiple actions in database

        public SqlConnection myConnection = new SqlConnection(connectionString);

        const string CREATE_TABLE = "CREATE TABLE ";

        SqlCommand command;
        SqlParameter p_userName;
        SqlParameter p_password;
        SqlParameter p_Email;
        SqlParameter p_ID;
        SqlParameter p_isActive;


        public const string userTableName = "Users";
        public const string activeUserTableName = "ActiveUsers";

        public const string catagoriesUsersString = " (Username text, Password text, Email text, userID int PRIMARY KEY, isActive int)";
        public const string catagoriesActiveUsersString = " (Username text, Password text, Email text, userID int PRIMARY KEY, IP text, Port int)";

        private DAL()
        {
            myConnection.Open();
        }

        public void insertUser(User user)
        {
            //string sqlCommandString = INSERT_INTO + userTableName + " " + VALUES + "(" +"'"+ user.name + COMMA + user.password + COMMA + user.mail + COMMA_NEXTINT + user.mail.GetHashCode() + COMMA_NEXTINT + user.GetIsActive() + ")";
            string sqlCommandString = @"INSERT INTO Users (Username, Password, Email, userID, isActive) VALUES (@userName, @userPassword, @userMail, @userID, @userIsActive)";
            command = new SqlCommand(sqlCommandString, myConnection);
            p_userName = command.Parameters.Add("@userName", SqlDbType.Text);
            p_password = command.Parameters.Add("@userPassword", SqlDbType.Text);
            p_Email = command.Parameters.Add("@userMail", SqlDbType.Text);
            p_ID = command.Parameters.Add("@userID", SqlDbType.Int);
            p_isActive = command.Parameters.Add("@userIsActive", SqlDbType.Int);
            p_userName.Value = user.name;
            p_password.Value = user.password;
            p_Email.Value = user.mail;
            p_ID.Value = user.mail.GetHashCode();
            p_isActive.Value = user.GetIsActive();
            command.ExecuteReader();

        }

        public bool isExist(User user)
        {
            string sqlCommandString = @"SELECT COUNT(*) FROM Users WHERE password LIKE @userPassword AND userID=@userID";
            command = new SqlCommand(sqlCommandString, myConnection);
            p_password = command.Parameters.Add("@userPassword", SqlDbType.Text);
            p_ID = command.Parameters.Add("@userID", SqlDbType.Int);
            p_password.Value = user.password;
            p_ID.Value = user.mail.GetHashCode();
            return (int)command.ExecuteScalar() == 1;
        }

        public void updateUserStatus(User user)
        {
            string sqlCommandString = @"UPDATE Users SET isActive = @userIsActive WHERE userID=@userID";
            command = new SqlCommand(sqlCommandString, myConnection);
            p_ID = command.Parameters.Add("@userID", SqlDbType.Int);
            p_isActive = command.Parameters.Add("@userIsActive", SqlDbType.Int);
            p_ID.Value = user.mail.GetHashCode();
            p_isActive.Value = user.GetIsActive();
            command.ExecuteReader();
        }

        public bool updateUser(User user)
        {
            try
            {
                //Username, Password, Email, userID, isActive
                SqlCommand cmd = new SqlCommand("UPDATE Users SET Username = @username, Password = @password WHERE userID = @userID", myConnection);
                cmd.Parameters.AddWithValue("@username", user.name);
                cmd.Parameters.AddWithValue("@password", user.password);
                cmd.Parameters.AddWithValue("@email", user.mail);
                cmd.Parameters.AddWithValue("@userID", user.mail.GetHashCode());
                //cmd.Parameters.AddWithValue("@isActive", user.isActive);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }

        /*public void insertActiveUser(ActiveUser user)
        {
            string sqlCommanString = INSERT_INTO + userTableName + catagoriesActiveUsersString + VALUES + user.name + COMMA + user.password + COMMA + user.mail + COMMA + user.mail.GetHashCode() + COMMA + user.ip + COMMA + user.port + ")";
            SqlCommand command = new SqlCommand(sqlCommanString, myConnection);
            command.ExecuteReader();
        }*/

        public bool deleteUser(User user)
        {
            try
            {
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM Users WHERE userID = @userID", myConnection);
                    //sc.Open();
                    //cmd.CommandText = "DELETE FROM Users WHERE userID = @userID";
                    cmd.Parameters.AddWithValue("@userID", user.mail.GetHashCode());
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }

        public string GetAllUsers()
        {
            List<User> list = new List<User>();
            string sqlCommandString = @"SELECT * FROM Users";
            command = new SqlCommand(sqlCommandString, myConnection);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var user = new User();
                    user.name = reader["Username"].ToString();
                    user.password = reader["Password"].ToString();
                    user.mail = reader["Email"].ToString();
                    list.Add(user);
                }
            }
            return JsonConvert.SerializeObject(list, Formatting.Indented);
        }

        public void createTable(string tableName, string catagoriesString)
        {
            string sqlCommanString = CREATE_TABLE + tableName + catagoriesString;
            command = new SqlCommand(sqlCommanString, myConnection);
            command.ExecuteReader();
        }

        public static DAL getInstance()
        {
            if (ins == null)
                ins = new DAL();
            return ins;
        }

    }
}
