using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    internal static class DbSeed
    {

        public static void seed() { 
            SqlConnection sqlConnection = DbConnection.get();
            createAdmin(sqlConnection);
            createBooks(sqlConnection);
        }


        private static void createAdmin(SqlConnection sqlConnection) {
            string createAdmin = "INSERT INTO USERS(USERNAME,PASS,FNAME,SNAME) " +
                    "VALUES ('admin','admin','Dimitar','Ivanov')";
            if (adminExist(sqlConnection) == false) {
                SqlCommand cmd = new SqlCommand(createAdmin, sqlConnection);
                cmd.ExecuteNonQuery();
            }          
        }

        private static void createBooks(SqlConnection sqlConnection)
        {
            string createBooks = "INSERT INTO BOOKS(ID,TITLE,AUTHOR,QUANTITY,TAKEN) " +
                    "VALUES " +
                    "(1,'Alice Adventures in Wonderland','Lewis Carroll',5,0)," +
                    "(2,'Harry Potter','J. K. Rowling',3,0)," +
                    "(3,'Twilight','Stephenie Meyer',4,0)," +
                    "(4,'Eragon','Christopher Paolini',3,0)";
            if (bookExist(sqlConnection, "Alice Adventures in Wonderland") == false){
                SqlCommand cmd = new SqlCommand(createBooks, sqlConnection);
                cmd.ExecuteNonQuery();
            }
            
        }


        private static bool adminExist(SqlConnection sqlConnection) {
            bool exist = false;
            string isAdminExist = @"
                IF EXISTS (
                    SELECT 1 
                    FROM USERS 
                    WHERE USERNAME = 'admin'
                )
                SELECT 1
                ELSE
                SELECT 0";

            SqlCommand command = new SqlCommand(isAdminExist, sqlConnection);          
            int recordExists = (int)command.ExecuteScalar();

            if (recordExists == 1){
                exist = true;
            }
            return exist;
        }

        private static bool bookExist(SqlConnection sqlConnection, string title) {
            bool exist = false;
            string isBookExist = $@"
                IF EXISTS (
                    SELECT 1 
                    FROM BOOKS 
                    WHERE TITLE = @value
                )
                SELECT 1
                ELSE
                SELECT 0";

            SqlCommand command = new SqlCommand(isBookExist, sqlConnection);
            command.Parameters.AddWithValue("@value", title);
            int recordExists = (int)command.ExecuteScalar();

            if (recordExists == 1){
                exist = true;
            }
            return exist;
        }
    }
}
