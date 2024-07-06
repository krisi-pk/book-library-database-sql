using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    internal class UserRepo
    {

        SqlConnection sqlConnection = DbConnection.get();
        public static bool LogUser(SqlConnection sqlConnection,string username,string password) {
            bool isLogin = false;
            if (userExist(sqlConnection, username) == false) {
                Console.WriteLine("This username doesn't exists");
            }
            else if (passEquals(sqlConnection, username, password) == false) {
                Console.WriteLine("Invalid pass");
            }
            else if (userExist(sqlConnection, username) == true &&
                passEquals(sqlConnection, username, password) == true) {
                isLogin = true;
            }
            return isLogin;
        }

        public static bool RegisterUser(SqlConnection sqlConnection, string user, string pass, string fName, string sName) {
            bool isRegister = false;
            string registerUser = @"INSERT INTO USERS(USERNAME,PASS,FNAME,SNAME) " +
                    "VALUES ('@user','@pass','@fName','@sName')";
            if (validateUsername(user) == true && validatePass(pass) == true &&
                validateName(fName) == true && validateName(sName) == true)
            {
                if (userExist(sqlConnection, user) == false)
                {
                    SqlCommand cmd = new SqlCommand(registerUser, sqlConnection);
                    cmd.Parameters.AddWithValue("@user", user);
                    cmd.Parameters.AddWithValue("@pass", pass);
                    cmd.Parameters.AddWithValue("@fName", fName);
                    cmd.Parameters.AddWithValue("@sName", sName);
                    cmd.ExecuteNonQuery();
                    isRegister = true;
                }
            }
            return isRegister;
        }

        public static void ShowUsers(SqlConnection sqlConnection) {
            string allUsers = "select * from USERS";
            SqlCommand cmd = new SqlCommand(allUsers, sqlConnection);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read()){
                Console.WriteLine($"{reader.GetInt32(0)} {reader.GetString(1)} {reader.GetString(2)}{reader.GetString(3)}{reader.GetString(4)}");
            }
            reader.Close();
        }


        private static bool passEquals(SqlConnection sqlConnection, string username, string pass)
        {
            bool equals = false;
            string corrPass = @"
                SELECT PASS FROM USERS
                WHERE USERNAME = @value
                ";

            SqlCommand command = new SqlCommand(corrPass, sqlConnection);
            command.Parameters.AddWithValue("@value", username);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read()) {
                string storedPassword = reader["PASS"].ToString();
                if (pass == storedPassword){
                    equals = true;
                }
            }
            reader.Close();
            return equals;
        }

        private static bool userExist(SqlConnection sqlConnection, string username)
        {
            bool exist = false;
            string isUserExist = @"
                IF EXISTS (
                    SELECT 1 
                    FROM USERS 
                    WHERE USERNAME = @value
                )
                SELECT 1
                ELSE
                SELECT 0";

            SqlCommand command = new SqlCommand(isUserExist, sqlConnection);
            command.Parameters.AddWithValue("@value", username);
            int recordExists = (int)command.ExecuteScalar();

            if (recordExists == 1){
                exist = true; 
            }
            return exist;
        }

        private static bool validateUsername(string username)
        {
            bool isCorrect = false;
            if ((username.Length >= 5) && (username.Length <= 10)) {
                isCorrect = true;
            }
            return isCorrect;
        }

        private static bool validatePass(string pass)
        {
            bool isCorrect = false;
            if ((pass.Length >= 5) && (pass.Length <= 20))
            {
                isCorrect = true;
            }
            return isCorrect;
        }

        private static bool validateName(string name)
        {
            bool isCorrect = false;
            if ((name.Length <= 50))
            {
                isCorrect = true;
            }
            return isCorrect;
        }
    }

}
